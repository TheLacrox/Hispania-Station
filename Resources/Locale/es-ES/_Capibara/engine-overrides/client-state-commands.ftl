# Cadenas de localización para comandos de estado de entidad y PVS del lado del cliente

cmd-reset-ent-help = Uso: resetent <Entity UID>
cmd-reset-ent-desc = Restablece una entidad al estado más reciente recibido del servidor. También restablecerá entidades que han sido separadas al espacio nulo.

cmd-reset-all-ents-help = Uso: resetallents
cmd-reset-all-ents-desc = Restablece todas las entidades al estado más reciente recibido del servidor. Solo afecta entidades que no han sido separadas al espacio nulo.

cmd-detach-ent-help = Uso: detachent <Entity UID>
cmd-detach-ent-desc = Separa una entidad al espacio nulo, como si hubiera salido del rango PVS.

cmd-local-delete-help = Uso: localdelete <Entity UID>
cmd-local-delete-desc = Elimina una entidad. A diferencia del comando delete normal, esto es DEL LADO DEL CLIENTE. A menos que la entidad sea del lado del cliente, probablemente causará errores.

cmd-full-state-reset-help = Uso: fullstatereset
cmd-full-state-reset-desc = Descarta toda la información de estado de entidades y solicita un estado completo del servidor.
