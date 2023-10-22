local cossetteBlockManager = {}

local colorNames = {
    ["Blue"] = 0,
    ["Rose"] = 1,
    ["Bright Sun"] = 2,
    ["Malachite"] = 3
}


cossetteBlockManager = {

    fieldInformation = {
        startWith = {
            fieldType = "integer",
            options = colorNames,
            editable = false
        }
    },

    name = "DzhakeHelper/CossetteBlockManager",
    depth = -100,
    texture = "objects/DzhakeHelper/CossetteBlockManager/idle",
    placements = {
        {
            name = "normal",
            data = {

                startWith = 0,
                everyDash = false,
            },
        },
    },
}




return cossetteBlockManager