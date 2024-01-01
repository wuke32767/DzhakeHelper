utils = require("utils")

local SequenceRefill = {
    name = "DzhakeHelper/SequenceRefill",
    depth = -100,
    texture = "objects/DzhakeHelper/sequenceRefill/idle00",
    width = 10,
    height = 10,
    placements = {
        name = "normal",
        data = {

            oneUse = false,
            useAnyway = false,
            staminaBased = true,
            respawnTime = 2.5,
        },
    },
    selection = function(room, entity)
        local x,y = entity.x or 0, entity.y or 0
        return utils.rectangle(x - 4, y - 4, 8,8)
    end
}




return SequenceRefill