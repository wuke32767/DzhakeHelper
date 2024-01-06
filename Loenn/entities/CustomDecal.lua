local CustomDecal = {
    name = "DzhakeHelper/CustomDecal",
    width = 10,
    height = 10,
    placements = {
        name = "normal",
        data = {
            imagePath = "_fallback",
            animated = false,
            scaleX = 1,
            scaleY = 1,
            rotation = 0,
            color = "FFFFFF",
            depth = 0,
        },
    },
    fieldInformation = {
        depth = {
            fieldType = "integer"
        },
        color = {
            fieldType = "color"
        },
    }
}

function CustomDecal.texture(room, entity)
    return entity.imagePath
end

return CustomDecal