local drawableLine = require("structs.drawable_line")
local drawableSprite = require("structs.drawable_sprite")
local utils = require("utils")
local logging = require("logging")
local connectedEntities = require("helpers.connected_entities")

local sequenceZipMover = {}

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

local ropeColors = {
    {110 / 255, 189 / 255, 245 / 255, 1.0},
    {194 / 255, 116 / 255, 171 / 255, 1.0},
    {227 / 255, 214 / 255, 148 / 255, 1.0},
    {128 / 255, 224 / 255, 141 / 255, 1.0}
}

sequenceZipMover.name = "DzhakeHelper/SequenceZipMover"
sequenceZipMover.minimumSize = {16, 16}
sequenceZipMover.nodeLimits = {1, -1}
sequenceZipMover.nodeVisibility = "never"
sequenceZipMover.fieldInformation = {
    index = {
        options = colorNames,
        editable = false,
        fieldType = "integer"
    },
    color = {
        fieldType = "color"
    }
}

sequenceZipMover.placements = {}
for i = 1, 4 do
    sequenceZipMover.placements[i] = {
        name = string.format("sequence_zipMover_%s", i - 1),
        data = {
            index = i - 1,
            width = 16,
            height = 16,
            blockedByPlayer = true,
            blockedByTheo = true,
            useCustomColor = false,
            color = "ffffff",
            imagePath = "objects/DzhakeHelper/sequenceZipMover/",
            backgroundBlock = true,
            delayBetweenNodes = 0.5,
            goBackByNodes = false,
        }
    }
end



local function getSearchPredicate(entity)
    return function(target)
        return entity._name == target._name and entity.index == target.index
    end
end

local function getTileSprite(entity, x, y, frame, color, depth, rectangles)
    local hasAdjacent = connectedEntities.hasAdjacent

    local drawX, drawY = (x - 1) * 8, (y - 1) * 8

    local closedLeft = hasAdjacent(entity, drawX - 8, drawY, rectangles)
    local closedRight = hasAdjacent(entity, drawX + 8, drawY, rectangles)
    local closedUp = hasAdjacent(entity, drawX, drawY - 8, rectangles)
    local closedDown = hasAdjacent(entity, drawX, drawY + 8, rectangles)
    local completelyClosed = closedLeft and closedRight and closedUp and closedDown

    local quadX, quadY = -1, -1

    if completelyClosed then
        if not hasAdjacent(entity, drawX + 8, drawY - 8, rectangles) then
            quadX, quadY = 24, 0
        elseif not hasAdjacent(entity, drawX - 8, drawY - 8, rectangles) then
            quadX, quadY = 24, 8
        elseif not hasAdjacent(entity, drawX + 8, drawY + 8, rectangles) then
            quadX, quadY = 24, 16
        elseif not hasAdjacent(entity, drawX - 8, drawY + 8, rectangles) then
            quadX, quadY = 24, 24
        else
            quadX, quadY = 8, 8
        end
    else
        if closedLeft and closedRight and not closedUp and closedDown then
            quadX, quadY = 8, 0
        elseif closedLeft and closedRight and closedUp and not closedDown then
            quadX, quadY = 8, 16
        elseif closedLeft and not closedRight and closedUp and closedDown then
            quadX, quadY = 16, 8
        elseif not closedLeft and closedRight and closedUp and closedDown then
            quadX, quadY = 0, 8
        elseif closedLeft and not closedRight and not closedUp and closedDown then
            quadX, quadY = 16, 0
        elseif not closedLeft and closedRight and not closedUp and closedDown then
            quadX, quadY = 0, 0
        elseif not closedLeft and closedRight and closedUp and not closedDown then
            quadX, quadY = 0, 16
        elseif closedLeft and not closedRight and closedUp and not closedDown then
            quadX, quadY = 16, 16
        end
    end

    if quadX ~= -1 and quadY ~= -1 then
        local sprite = drawableSprite.fromTexture(frame, entity)

        sprite:addPosition(drawX, drawY)
        sprite:useRelativeQuad(quadX, quadY, 8, 8)
        sprite:setColor(color)

        sprite.depth = depth

        return sprite
    end
end


function sequenceZipMover.sprite(room, entity)
    local sprites = {}
    
    local i = entity.index or 0

    local relevantBlocks = utils.filter(getSearchPredicate(entity), room.entities)

    connectedEntities.appendIfMissing(relevantBlocks, entity)

    local rectangles = connectedEntities.getEntityRectangles(relevantBlocks)

    

    local width, height = entity.width or 32, entity.height or 32
    local tileWidth, tileHeight = math.ceil(width / 8), math.ceil(height / 8)

    local color = colors[i + 1]
    local frame = "objects/DzhakeHelper/sequenceZipMover/solid"
    local depth = -10

    for x = 1, tileWidth do
        for y = 1, tileHeight do
            local sprite = getTileSprite(entity, x, y, frame, color, depth, rectangles)

            if sprite then
                table.insert(sprites, sprite)
            end
        end
    end

    local x, y = entity.x or 0, entity.y or 0
    local halfWidth, halfHeight = math.floor(width / 2), math.floor(height / 2)
    local centerX, centerY = x + halfWidth, y + halfHeight

    local ropeColor = colors[i + 1]

    local nodes = entity.nodes or {{x = 0, y = 0}}

    local nodeSprites = {}


    local cx, cy = centerX, centerY
    for _, node in ipairs(nodes) do
        local centerNodeX, centerNodeY = node.x + halfWidth, node.y + halfHeight

        local nodeCogSprite = drawableSprite.fromTexture("objects/DzhakeHelper/sequenceZipMover/cog")
        nodeCogSprite:setColor(color)

        nodeCogSprite:setPosition(centerNodeX, centerNodeY)
        nodeCogSprite:setJustification(0.5, 0.5)

        local points = {cx, cy, centerNodeX, centerNodeY}
        local leftLine = drawableLine.fromPoints(points, ropeColor, 1)
        local rightLine = drawableLine.fromPoints(points, ropeColor, 1)

        leftLine:setOffset(0, 4.5)
        rightLine:setOffset(0, -4.5)

        leftLine.depth = 5000
        rightLine.depth = 5000

        for _, sprite in ipairs(leftLine:getDrawableSprite()) do
            table.insert(sprites, sprite)
        end

        for _, sprite in ipairs(rightLine:getDrawableSprite()) do
            table.insert(sprites, sprite)
        end

        table.insert(sprites, nodeCogSprite)

        cx, cy = centerNodeX, centerNodeY
    end


    return sprites
end

function sequenceZipMover.selection(room, entity)
    local x, y = entity.x or 0, entity.y or 0
    local width, height = entity.width or 8, entity.height or 8
    local halfWidth, halfHeight = math.floor(entity.width / 2), math.floor(entity.height / 2)

    local mainRectangle = utils.rectangle(x, y, width, height)

    local nodes = entity.nodes or {{x = 0, y = 0}}
    local nodeRectangles = {}
    for _, node in ipairs(nodes) do
        local centerNodeX, centerNodeY = node.x + halfWidth, node.y + halfHeight

        table.insert(nodeRectangles, utils.rectangle(centerNodeX - 5, centerNodeY - 5, 10, 10))
    end

    return mainRectangle, nodeRectangles
end

return sequenceZipMover