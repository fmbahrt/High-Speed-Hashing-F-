#r "hashtable.dll"

// Create a random stream of data for testing purposes
let rnd = System.Random(1337)

// I want the stream to consists of uint64 * uint64 tuples
let n = 500000
let stream = Array.init<uint64*uint64> n (fun idx -> (uint64(rnd.Next(1,4096)),uint64(rnd.Next(1, 100))))

let kvadratsum (table : highspeedhashing.HashTable.table) stream =
    for (x, v) in stream do
        let squared = v * v
        highspeedhashing.HashTable.increment table x squared

let mmptable = highspeedhashing.HashTable.constructMMP 4
let mstable = highspeedhashing.HashTable.constructMS 4

let runtest n idrange (table : highspeedhashing.HashTable.table) =
    let stream = Array.init<uint64*uint64> n (fun idx -> 
        (uint64(rnd.Next(1,idrange)),uint64(rnd.Next(1, 100))))

    kvadratsum table stream
    
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    kvadratsum table stream
    stopwatch.Stop()
    printfn "N: %A Elapsed: %f" n stopwatch.Elapsed.TotalMilliseconds


printfn "LIST SIZE TEST"
printfn "Multiply Mod Prime PERFORMANCE"
runtest 128 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 256 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 512 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 1024 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 2048 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 4096 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 8192 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 16384 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 65536 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 131072 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 262144 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 256 (highspeedhashing.HashTable.constructMMP 8)

printfn "Multiply Shift PERFORMANCE"
runtest 128 256 (highspeedhashing.HashTable.constructMS 8)
runtest 256 256 (highspeedhashing.HashTable.constructMS 8)
runtest 512 256 (highspeedhashing.HashTable.constructMS 8)
runtest 1024 256 (highspeedhashing.HashTable.constructMS 8)
runtest 2048 256 (highspeedhashing.HashTable.constructMS 8)
runtest 4096 256 (highspeedhashing.HashTable.constructMS 8)
runtest 8192 256 (highspeedhashing.HashTable.constructMS 8)
runtest 16384 256 (highspeedhashing.HashTable.constructMS 8)
runtest 65536 256 (highspeedhashing.HashTable.constructMS 8)
runtest 131072 256 (highspeedhashing.HashTable.constructMS 8)
runtest 262144 256 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 256 (highspeedhashing.HashTable.constructMS 8)

printfn "ID RANGE TEST"
printfn "Multiply Mod Prime PERFORMANCE"
runtest 1048576 128 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 256 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 512 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 1024 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 2048 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 4096 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 8192 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 16384 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 65536 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 131072 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 262144 (highspeedhashing.HashTable.constructMMP 8)
runtest 1048576 1048576 (highspeedhashing.HashTable.constructMMP 8)

printfn "Multiply Shift PERFORMANCE"
runtest 1048576 128 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 256 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 512 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 1024 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 2048 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 4096 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 8192 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 16384 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 65536 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 131072 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 262144 (highspeedhashing.HashTable.constructMS 8)
runtest 1048576 1048576 (highspeedhashing.HashTable.constructMS 8)

printfn "ID RANGE TEST WITH LARGER HASH TABLE"
printfn "Multiply Mod Prime PERFORMANCE"
runtest 1048576 128 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 256 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 512 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 1024 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 2048 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 4096 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 8192 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 16384 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 65536 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 131072 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 262144 (highspeedhashing.HashTable.constructMMP 16)
runtest 1048576 1048576 (highspeedhashing.HashTable.constructMMP 16)

printfn "Multiply Shift PERFORMANCE"
runtest 1048576 128 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 256 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 512 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 1024 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 2048 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 4096 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 8192 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 16384 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 65536 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 131072 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 262144 (highspeedhashing.HashTable.constructMS 16)
runtest 1048576 1048576 (highspeedhashing.HashTable.constructMS 16)
