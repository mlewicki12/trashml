
class Card define
    member suit = 1
    member value = 5

    member str do
        return suit + " " + value
    end
end

print card.suit
