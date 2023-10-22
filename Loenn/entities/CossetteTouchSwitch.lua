local drawableSprite = require("structs.drawable_sprite")
local utils = require("utils")

local CossetteTouchSwitch = {}


local iconsTextures = {
    ["rhombus"] = "objects/DzhakeHelper/cossetteTouchSwitch/icons/rhombus/icon",
    ["rectangle"] = "objects/DzhakeHelper/cossetteTouchSwitch/icons/rectangle/icon",
    ["square"] = "objects/DzhakeHelper/cossetteTouchSwitch/icons/square/icon",
    ["original"] = "objects/touchswitch/icon"
}


local containerTexture = "objects/DzhakeHelper/cossetteTouchSwitch/container/container"

CossetteTouchSwitch.fieldInformation = {
    iconTexture = {
        fieldType = "string",
        options = iconsTextures,
        editable = true
    },
    group = {
        fieldType = "integer",
        minimumValue = 0,
    }
}

CossetteTouchSwitch.name = "DzhakeHelper/CossetteTouchSwitch"
CossetteTouchSwitch.depth = 2000
CossetteTouchSwitch.width = 14
CossetteTouchSwitch.height = 14
CossetteTouchSwitch.placements = {
    {
        name = "normal",
        data = {
            group = 1,
            iconTexture = "objects/DzhakeHelper/cossetteTouchSwitch/icons/rhombus/icon",
            groundReset = false,
        },
    }
}



function CossetteTouchSwitch.sprite(room, entity)
    local containerSprite = drawableSprite.fromTexture(containerTexture, entity)
    local iconSprite = drawableSprite.fromTexture(entity.iconTexture.."00", entity)

    return {containerSprite, iconSprite}
end

function CossetteTouchSwitch.selection(room, entity)
    local x,y = entity.x or 0, entity.y or 0
    return utils.rectangle(x-7,y-7,14,14)
end

return CossetteTouchSwitch