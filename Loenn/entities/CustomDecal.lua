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
        },
    },
    fieldInformation = {
        depth = {
            fieldType = "integer"
        },
        color = {
            fieldType = "color"
        },
    },
    fieldOrder = {
        "x","y","imagePath","animationName","delay","depth","scaleX","scaleY","rotation","color","flag","updateSpriteOnlyIfFlag","inversedFlag"
    }
}

function CustomDecal.texture(room, entity)
    if entity.animated then return entity.imagePath..entity.animationName.."00" end
    return entity.imagePath
end

return CustomDecal