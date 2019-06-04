#r "hashtable.dll"

let k = 8
let a0 = highspeedhashing.HashTable.rnd88
let a1 = highspeedhashing.HashTable.rnd88
let a2 = highspeedhashing.HashTable.rnd88
let a3 = highspeedhashing.HashTable.rnd88


// Create a random stream of data for testing purposes
let rnd = System.Random(1337)

// I want the stream to consists of uint64 * uint64 tuples
let n = 100000
let stream = Array.init<uint64*uint64> n (fun idx ->
    (uint64(rnd.Next(1,512)),uint64(rnd.Next(1, 16))))


let bench (k : int) = 
    // Normal hashing for ground truth
    let mstable = highspeedhashing.HashTable.constructMS 8

    // Buckets
    let h = highspeedhashing.Hash.fouruniversalcountsketch a0 a1 a2 a3 k
    let C = Array.init<int64> k (fun idx -> int64 0)

    for (x, v) in stream do
        let (h, s) = h x
        C.[(int h)] <- C.[(int h)] + (s * int64(v * v))
        
        // Insert into ground truth table
        highspeedhashing.HashTable.increment mstable x (v*v)

    // Calculate error to ground truth

    // get unique keys from stream
    let stream_keys = Array.init<uint64> n (fun idx -> fst stream.[idx])

    let keys = Array.ofSeq (set stream_keys)

    let mutable cum_error : int64 = int64 0
    for key in keys do
        
        // Get estimate
        let (h, s) = h key
        let q_hat = s * C.[(int h)]

        // Get ground thruth
        let q_truth = highspeedhashing.HashTable.get mstable key

        let error = abs (int64(q_truth) - q_hat)
        cum_error <- cum_error + error

    let avg_error = cum_error / (int64 (Array.length keys))
    printfn "avg_error: %A" avg_error

bench 4
bench 8
bench 16
bench 32
bench 64
bench 128
bench 256
