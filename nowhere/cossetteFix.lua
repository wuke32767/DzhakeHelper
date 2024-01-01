local script = {
    name = "cossetteFix",
    displayName = "Cossette Fix",
    tooltip = "Turn cossette entities into sequence entities"
}

function script.run(room, args)
    
    for _, entity in ipairs(room.entities) do
        if string.sub(entity._name,1,21) == "DzhakeHelper/Cossette" then
            entity._name = "DzhakeHelper/Sequence"..string.sub(entity._name,22,#entity._name)
        end
    end


end

return script