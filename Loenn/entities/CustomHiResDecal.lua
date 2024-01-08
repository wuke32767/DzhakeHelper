local spriteBanks = {
    ["Game"] = 0,
    ["Gui"] = 1,
    ["Portraits"] = 2,
    ["Misc"] = 3
}

local CustomHiResDecal = {
    name = "DzhakeHelper/CustomHiResDecal",
    texture = "objects/DzhakeHelper/customHiResDecal/preview",
    placements = {
        name = "normal",
        data = {
            imagePath = "objects/DzhakeHelper/customHiResDecal/default",
            scaleX = 1,
            scaleY = 1,
            rotation = 0,
            color = "FFFFFF",
            depth = 0,
            pathStart = 0,
        },
    },
    fieldInformation = {
        depth = {
            fieldType = "integer"
        },
        color = {
            fieldType = "color"
        },
        pathStart = {
            fieldType = "integer",
            editable = false,
            options = spriteBanks
        },
    }
}

return CustomHiResDecal