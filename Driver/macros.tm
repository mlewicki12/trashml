# macro test
macro test do
    if x > 6 do
        print x
    end else do
        print 6
    end
end

let x = 5
test

# what about this one
let x = 7
test

macro printX do
    repeat 1 to x do
        print x
    end
end

let x = 5
printX
# does this comment work?