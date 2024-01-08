utils = require("utils")

local VoidLift = {
    name = "DzhakeHelper/VoidLift",
    depth = 100,
    texture = "objects/DzhakeHelper/voidLift/idle00",
    placements = {
        name = "normal",
        data = {
            newSpeed = -300,
            refillDash = false,
            dashToLeft = true,
            dashToRight = true,
            dashToTop = false,
            dashToBottom = false,
            dashToBottomRight = false,
            dashToBottomLeft = false,
            dashToTopRight = false,
            dashToTopLeft = false,
        },
    },
    selection = function(room, entity)
        local x,y = entity.x or 0, entity.y or 0
        return utils.rectangle(x - 7, y - 7, 14,14)
    end
}




return VoidLift