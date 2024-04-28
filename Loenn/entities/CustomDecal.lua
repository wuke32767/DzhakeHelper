local spriteBanks = {
    ["Game"] = 0,
    ["Gui"] = 1,
    ["Portraits"] = 2,
    ["Misc"] = 3
}

local CustomDecal = {
    name = "DzhakeHelper/CustomDecal",
    placements = {
        name = "normal",
        data = {
            imagePath = "_fallback",
            animated = false,
            animationName = "idle",
            delay = 0.1,
            scaleX = 1,
            scaleY = 1,
            rotation = 0,
            color = "FFFFFF",
            depth = 0,
            flag = "",
            updateSpriteOnlyIfFlag = true,
            inversedFlag = false,
            pathRoot = 0,
            hiRes = false,
            attached = false,
        },
    },
    fieldInformation = {
        depth = {
            fieldType = "integer"
        },
        color = {
            fieldType = "color"
        },
        pathRoot = {
            fieldType = "integer",
            editable = false,
            options = spriteBanks
        }
    },
    fieldOrder = {
        "x","y","pathRoot","imagePath","animationName","delay","depth","scaleX","scaleY","rotation","color","flag","attached","hiRes","updateSpriteOnlyIfFlag","inversedFlag"
    }
}

function CustomDecal.texture(room, entity)
    if entity.hiRes then return "objects/DzhakeHelper/customHiResDecal/preview" end
    if entity.animated then return entity.imagePath..entity.animationName.."00" end
    return entity.imagePath
end

return CustomDecal