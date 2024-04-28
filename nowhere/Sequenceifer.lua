utils = require("utils")
local dzhakeHelper = require("mods").requireFromPlugin("libraries.dzhake_helper")

local colors = {
    {92 / 255, 91 / 255, 218 / 255},
    {255 / 255, 0 / 255, 81 / 255},
    {215 / 255, 215 / 255, 0 / 255},
    {73 / 255, 220 / 255, 136 / 255},
}

local colorNames = {
    ["Blue"] = 0,
    ["Rose"] = 1,
    ["Bright Sun"] = 2,
    ["Malachite"] = 3
}

local Sequenceifer = {
    name = "DzhakeHelper/Sequenceifer",
    depth = -100,
    width = 10,
    height = 10,
    placements = {
        name = "normal",
        data = {
            index = 0,
            useCustomColor = false,
            customColor = "FFFFFF",
        },
    },
    fieldInformation = {
        index = {
            fieldType = "integer",
            options = colorNames,
            editable = false
        },
        color = {
            fieldType = "color"
        }
    },
    sprite = function(room, entity)
        local relevantBlocks = utils.filter(dzhakeHelper.getSequenceBlocksSearchPredicate(entity), room.entities)

        connectedEntities.appendIfMissing(relevantBlocks, entity)

        local rectangles = connectedEntities.getEntityRectangles(relevantBlocks)

        local sprites = {}

        local width, height = entity.width or 32, entity.height or 32
        local tileWidth, tileHeight = math.ceil(width / 8), math.ceil(height / 8)

        local index = entity.index or 0
        local color = colors[index + 1] or colors[1]
        if entity.useCustomColor then color = entity.color end
        local frame = "objects/DzhakeHelper/sequenceifer/outline"
        local depth = -300

        for x = 1, tileWidth do
            for y = 1, tileHeight do
                local sprite = dzhakeHelper.getTileSprite(entity, x, y, frame, color, depth, rectangles)

                if sprite then
                    table.insert(sprites, sprite)
                end
            end
        end

        return sprites
    end
}




return Sequenceifer