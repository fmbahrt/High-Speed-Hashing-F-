#r "hashtable.dll"

let k = 64
let a0 = highspeedhashing.HashTable.rnd88
let a1 = highspeedhashing.HashTable.rnd88
let a2 = highspeedhashing.HashTable.rnd88
let a3 = highspeedhashing.HashTable.rnd88

let h = highspeedhashing.Hash.fouruniversalcountsketch a0 a1 a2 a3 k

// Create a random stream of data for testing purposes
let rnd = System.Random(1337)

// I want the stream to consists of uint64 * uint64 tuples
let n = 1000
let stream = Array.init<uint64*uint64> n (fun idx ->
    (uint64(rnd.Next(1,4096)),uint64(rnd.Next(1, 100))))

// Buckets
let C = Array.init<int64> k (fun idx -> int64 0)

for (x, v) in stream do
    let (h, s) = h x
    C.[(int h)] <- s * int64(v * v)
