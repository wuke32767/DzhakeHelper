local script = {
    name = "customDecalsConverter",
    displayName = "Custom Decal converter",
    tooltip = "Turn old version of Custom Decals to new version"
}

function script.run(room, args)
    
    for _, entity in ipairs(room.entities) do
        if entity._name == "DzhakeHelper/CustomDecal" and entity.imagePath ~= nil then
            entity.removeDecalsFromPath = true

            entity.flags = entity.flag
            entity.flag = nil

            entity.texture = entity.imagePath..entity.animationName
            entity.imagePath = nil
            entity.animationName = nil
            entity.animated = nil
            entity.delay = nil

            entity.inversedFlag = nil
            entity.attached = nil
        end
    end


end

return script