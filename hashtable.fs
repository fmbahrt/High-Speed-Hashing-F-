namespace highspeedhashing

module Hash =
    let primePow = 89
    let MP : bigint = bigint(2) ** primePow - bigint(1)

    let fouruniversalcountsketch (a0 : bigint) (a1 : bigint)
                                 (a2 : bigint) (a3 : bigint) (k : int32) = 
        let h (x : uint64) : uint64 * int64 = 
            let x : bigint = bigint x
            // Use horners rules to compute polynomial
            let yz = a0 + x * (a1 + x * (a2 + x * a3))
            // Same mod trick as multiply mod prime 
            //  love duplicated code TODO
            let y0 : bigint = yz &&& MP
            let y1 : bigint = yz >>> primePow
            
            let mutable y : bigint = y0 + y1
            if (y >= MP) then y <- y - MP

            // Now, lean back, and follow the recipe

            // Assume that k is a power of two
            let yprime : bigint = y &&& (bigint (2 * k - 1))
            let h : bigint = yprime >>> 1
            let b : bigint = yprime &&& (bigint 1)
            let s : bigint = (bigint 1) - (bigint 2) * b

            let hx : uint64 = uint64 h
            let sx : int64 = int64 s // Does not have to be int64 (only sign)
            // Now cast to uint64 - because F# and collections
            (hx, sx)
        h

    let multiplymodprime (a : bigint) (b : bigint) (l : int32) =
        let h (x : uint64) =
            let x : bigint = bigint x
            let c : bigint = x * a + b
            
            let y0 : bigint = c &&& MP
            let y1 : bigint = c >>> primePow
           
            let mutable y : bigint = y0 + y1
            if (y >= MP) then y <- y - MP

            let lmask : bigint = bigint(2) ** l - bigint(1)
            let ret            = y &&& lmask
            uint64 ret
        h

    let multiplyshift (a : uint64) (l : int32) =
        // TODO: assert size of l
        // TODO: assert a odd
        let leshift : int32 = 64
        let h (x : uint64) =
            // Pls overflow
            let prod : uint64 = a * x
            let shif : int32 = leshift - l
            let ret  : uint64 = prod >>> shif
            ret
        h

module LeChain = 
    type LinkedList =
        | Empty
        | Cons of head:(uint64*uint64) * tail: LinkedList

    let rec get (l : LinkedList) (x : uint64) =
        match l with
        | Empty -> uint64 0
        | Cons (head, tail) ->
                if (fst head) = x then
                    snd head
                else
                    get tail x

    let rec set (l : LinkedList) (x : uint64) (v : uint64) : LinkedList = 
        match l with
        | Empty -> Cons ((x, v), Empty)
        | Cons (head, tail) ->
                if (fst head) = x then
                    Cons ((x, v), tail)
                else
                    Cons (head, (set tail x v))

    let increment (l : LinkedList) (x : uint64) (d : uint64) : LinkedList =
        let value : uint64 = get l x
        let sum   : uint64 = value + d
        set l x sum

    let rec printlist (l : LinkedList) =
        match l with
        | Empty ->
                printfn "Empty..."
        | Cons (head, tail) ->
                let i : uint64 = fst head
                let v : uint64 = snd head
                printfn "Identifier: %A, Value: %A" i v
                printlist tail

module HashTable =
    type table = {
        t: array<LeChain.LinkedList>;
        h: uint64 -> uint64;
    }

    let rnd88 =
        let rnd = System.Random()
        let a = bigint (rnd.Next())
        let b = bigint (rnd.Next())
        let c = bigint (rnd.Next())

        let big1 = a <<< 32
        let big2 = big1 + b

        let big3 = big2 <<< 24
        let big4 = c >>> 8
        big3 + big4

    let rnd64 = 
        let rnd = System.Random()
        let a = uint64 (rnd.Next())
        let b = uint64 (rnd.Next())
        (a <<< 32) + b

    let construct (hf : uint64 -> uint64) (l : int32) =
        let size = 1 <<< l
        let t : table = {
            t=(Array.init<LeChain.LinkedList> size (fun idx -> LeChain.Empty));
            h=hf;
        }
        t
    
    let constructMMP (l : int32) =
        // Generator random coefficient a (88 random bits)
        let a = rnd88
        let b = rnd88
        let h = Hash.multiplymodprime a b l
        construct h l

    let constructMS (l : int32) = 
        let mutable a = rnd64

        // a has to be odd
        let one = 1UL
        let two = 2UL
        while (a % two) = one do
            a <- rnd64

        let h = Hash.multiplyshift a l
        construct h l

    let get (t : table) (x : uint64) = 
        // idx has to be int32 ://
        let idx = int (t.h x)
        let chain = t.t.[idx]
        let res = LeChain.get chain x
        res

    let set (t : table) (x : uint64) (v : uint64) = 
        let idx = int (t.h x)
        let chain = t.t.[idx]
        t.t.[idx] <- LeChain.set chain x v 
    
    let increment (t : table) (x : uint64) (d : uint64) =
        let idx = int (t.h x)
        let chain = t.t.[idx]
        t.t.[idx] <- LeChain.increment chain x d
