local fakeTilesHelper = require("helpers.fake_tiles")
local utils = require("utils")
local matrixLib = require("utils.matrix")
local drawableSprite = require("structs.drawable_sprite")
local connectedEntities = require("helpers.connected_entities")
local dzhakeHelper = require("mods").requireFromPlugin("libraries.dzhake_helper")



local sequenceBlock = {}

local colors = {
    {92 / 255, 91 / 255, 218 / 255},
    {255 / 255, 0 / 255, 81 / 255},
    {215 / 255, 215 / 255, 0 / 255},
    {73 / 255, 220 / 255, 136 / 255},
}

local frames = {
    "objects/DzhakeHelper/sequenceBlock/solid",
    "objects/DzhakeHelper/sequenceBlock/solid",
    "objects/DzhakeHelper/sequenceBlock/solid",
    "objects/DzhakeHelper/sequenceBlock/solid",
}

local depths = {
    -10,
    -10,
    -10,
    -10
}

local colorNames = {
    ["Blue"] = 0,
    ["Rose"] = 1,
    ["Bright Sun"] = 2,
    ["Malachite"] = 3
}

sequenceBlock.name = "DzhakeHelper/SequenceBlock"
sequenceBlock.minimumSize = {16, 16}
sequenceBlock.fieldInformation = {
    index = {
        fieldType = "integer",
        options = colorNames,
        editable = false
    },
    color = {
        fieldType = "color"
    }
}
sequenceBlock.placements = {}

for i, _ in ipairs(colors) do
    sequenceBlock.placements[i] = {
        name = string.format("sequence_block_%s", i - 1),
        data = {
            index = i - 1,
            width = 16,
            height = 16,
            blockedByPlayer = true,
            blockedByTheo = true,
            useCustomColor = false,
            color = "ffffff",
            imagePath = "objects/DzhakeHelper/sequenceBlock/",
            backgroundBlock = true,
        }
    }
end

function sequenceBlock.sprite(room, entity)
    local relevantBlocks = utils.filter(dzhakeHelper.getSequenceBlocksSearchPredicate(entity), room.entities)

    connectedEntities.appendIfMissing(relevantBlocks, entity)

    local rectangles = connectedEntities.getEntityRectangles(relevantBlocks)

    local sprites = {}

    local width, height = entity.width or 32, entity.height or 32
    local tileWidth, tileHeight = math.ceil(width / 8), math.ceil(height / 8)

    local index = entity.index or 0
    local color = colors[index + 1] or colors[1]
    if entity.useCustomColor then color = entity.color end
    local frame = entity.imagePath.."solid" or frames[index + 1] or frames[1]
    local depth = depths[index + 1] or depths[1]

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

return sequenceBlock