
class Card define
    member suit
    member value

    member con with st, val do
        let suit = st
        let value = val
    end

    member str do
        return suit + " " + value
    end
end

# 10 of diamonds
let card = new Card with st = 1, val = 10
print card.suit

# this will not work, because macros aren't handled yet
print card.str
