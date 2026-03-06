### Localización de comandos de consola del motor

cmd-hint-float = [float]

## errores genéricos de comandos

cmd-invalid-arg-number-error = Número de argumentos inválido.

cmd-parse-failure-integer = {$arg} no es un entero válido.
cmd-parse-failure-float = {$arg} no es un float válido.
cmd-parse-failure-bool = {$arg} no es un bool válido.
cmd-parse-failure-uid = {$arg} no es un UID de entidad válido.
cmd-parse-failure-mapid = {$arg} no es un MapId válido.
cmd-parse-failure-enum = {$arg} no es un Enum {$enum}.
cmd-parse-failure-grid = {$arg} no es una grilla válida.
cmd-parse-failure-cultureinfo = "{$arg}" no es un CultureInfo válido.
cmd-parse-failure-entity-exist = El UID {$arg} no corresponde a una entidad existente.
cmd-parse-failure-session = No existe una sesión con el nombre de usuario: {$username}

cmd-error-file-not-found = No se pudo encontrar el archivo: {$file}.
cmd-error-dir-not-found = No se pudo encontrar el directorio: {$dir}.

cmd-failure-no-attached-entity = No hay ninguna entidad asociada a esta terminal.

## comando 'help'
cmd-help-desc = Muestra ayuda general o texto de ayuda para un comando específico
cmd-help-help = Uso: help [nombre del comando]
    Cuando no se proporciona un nombre de comando, muestra texto de ayuda general. Si se proporciona un nombre de comando, muestra el texto de ayuda de ese comando.

cmd-help-no-args = Para mostrar ayuda de un comando específico, escribe 'help <comando>'. Para listar todos los comandos disponibles, escribe 'list'. Para buscar comandos, usa 'list <filtro>'.
cmd-help-unknown = Comando desconocido: { $command }
cmd-help-top = { $command } - { $description }
cmd-help-invalid-args = Cantidad de argumentos inválida.
cmd-help-arg-cmdname = [nombre del comando]

## comando 'cvar'
cmd-cvar-desc = Obtiene o establece un CVar.
cmd-cvar-help = Uso: cvar <nombre | ?> [valor]
    Si se pasa un valor, se analiza y almacena como el nuevo valor del CVar.
    Si no, se muestra el valor actual del CVar.
    Usa 'cvar ?' para obtener una lista de todos los CVars registrados.

cmd-cvar-invalid-args = Debe proporcionar exactamente uno o dos argumentos.
cmd-cvar-not-registered = El CVar '{ $cvar }' no está registrado. Usa 'cvar ?' para obtener una lista de todos los CVars registrados.
cmd-cvar-parse-error = El valor de entrada tiene un formato incorrecto para el tipo { $type }
cmd-cvar-compl-list = Listar CVars disponibles
cmd-cvar-arg-name = <nombre | ?>
cmd-cvar-value-hidden = <valor oculto>

## comando 'cvar_subs'
cmd-cvar_subs-desc = Lista las suscripciones OnValueChanged de un CVar.
cmd-cvar_subs-help = Uso: cvar_subs <nombre>

cmd-cvar_subs-invalid-args = Debe proporcionar exactamente un argumento.
cmd-cvar_subs-arg-name = <nombre>

## comando 'list'
cmd-list-desc = Lista los comandos disponibles, con filtro de búsqueda opcional
cmd-list-help = Uso: list [filtro]
    Lista todos los comandos disponibles. Si se proporciona un argumento, se usará para filtrar comandos por nombre.

cmd-list-heading = LADO NOMBRE            DESC{"\u000A"}-------------------------{"\u000A"}

cmd-list-arg-filter = [filtro]

## comando '>', ejecución remota
cmd-remoteexec-desc = Ejecuta comandos del lado del servidor
cmd-remoteexec-help = Uso: > <comando> [arg] [arg] [arg...]
    Ejecuta un comando en el servidor. Esto es necesario si existe un comando con el mismo nombre en el cliente, ya que ejecutar el comando directamente ejecutaría primero el del cliente.

## comando 'gc'
cmd-gc-desc = Ejecuta el GC (Recolector de Basura)
cmd-gc-help = Uso: gc [generación]
    Usa GC.Collect() para ejecutar el Recolector de Basura.
    Si se proporciona un argumento, se analiza como número de generación del GC y se usa GC.Collect(int).
    Usa el comando 'gfc' para hacer un GC completo con compactación del LOH.
cmd-gc-failed-parse = Error al analizar el argumento.
cmd-gc-arg-generation = [generación]

## comando 'gcf'
cmd-gcf-desc = Ejecuta el GC completamente, compactando LOH y todo.
cmd-gcf-help = Uso: gcf
    Hace un GC.Collect(2, GCCollectionMode.Forced, true, true) completo, también compactando el LOH.
    Esto probablemente bloqueará durante cientos de milisegundos, ten cuidado.

## comando 'gc_mode'
cmd-gc_mode-desc = Cambia/Lee el modo de latencia del GC
cmd-gc_mode-help = Uso: gc_mode [tipo]
    Si no se proporciona argumento, devuelve el modo de latencia actual del GC.
    Si se pasa un argumento, se analiza como GCLatencyMode y se establece como modo de latencia del GC.

cmd-gc_mode-current = modo de latencia actual del GC: { $prevMode }
cmd-gc_mode-possible = modos posibles:
cmd-gc_mode-option = - { $mode }
cmd-gc_mode-unknown = modo de latencia del GC desconocido: { $arg }
cmd-gc_mode-attempt = intentando cambiar modo de latencia del GC: { $prevMode } -> { $mode }
cmd-gc_mode-result = modo de latencia del GC resultante: { $mode }
cmd-gc_mode-arg-type = [tipo]

## comando 'mem'
cmd-mem-desc = Muestra información de memoria administrada
cmd-mem-help = Uso: mem

cmd-mem-report = Tamaño del Heap: { TOSTRING($heapSize, "N0") }
    Total Asignado: { TOSTRING($totalAllocated, "N0") }

## comando 'physics'
cmd-physics-overlay = {$overlay} no es una capa de depuración reconocida

## comando 'lsasm'
cmd-lsasm-desc = Lista los ensamblados cargados por contexto de carga
cmd-lsasm-help = Uso: lsasm

## comando 'exec'
cmd-exec-desc = Ejecuta un archivo de script desde los datos de usuario escribibles del juego
cmd-exec-help = Uso: exec <nombreArchivo>
    Cada línea del archivo se ejecuta como un comando individual, a menos que empiece con #

cmd-exec-arg-filename = <nombreArchivo>

## comando 'dump_net_comps'
cmd-dump_net_comps-desc = Imprime la tabla de componentes en red.
cmd-dump_net_comps-help = Uso: dump_net_comps

cmd-dump_net_comps-error-writeable = El registro aún es escribible, los IDs de red no se han generado.
cmd-dump_net_comps-header = Registros de Componentes en Red:

## comando 'dump_event_tables'
cmd-dump_event_tables-desc = Imprime las tablas de eventos dirigidos para una entidad.
cmd-dump_event_tables-help = Uso: dump_event_tables <entityUid>

cmd-dump_event_tables-missing-arg-entity = Falta el argumento de entidad
cmd-dump_event_tables-error-entity = Entidad inválida
cmd-dump_event_tables-arg-entity = <entityUid>

## comando 'monitor'
cmd-monitor-desc = Activa/desactiva un monitor de depuración en el menú F3.
cmd-monitor-help = Uso: monitor <nombre>
    Monitores posibles: { $monitors }
    También puedes usar los valores especiales "-all" y "+all" para ocultar o mostrar todos los monitores, respectivamente.

cmd-monitor-arg-monitor = <monitor>
cmd-monitor-invalid-name = Nombre de monitor inválido
cmd-monitor-arg-count = Falta el argumento de monitor
cmd-monitor-minus-all-hint = Oculta todos los monitores
cmd-monitor-plus-all-hint = Muestra todos los monitores


## comando 'setambientlight'
cmd-set-ambient-light-desc = Permite establecer la luz ambiental para el mapa especificado, en SRGB.
cmd-set-ambient-light-help = setambientlight [mapid] [r g b a]
cmd-set-ambient-light-parse = No se pudieron analizar los argumentos como valores de byte para un color.

## Comandos de Mapeo

cmd-savemap-desc = Serializa un mapa al disco. No guardará un mapa post-init a menos que se fuerce.
cmd-savemap-help = savemap <MapID> <Ruta> [forzar]
cmd-savemap-not-exist = El mapa de destino no existe.
cmd-savemap-init-warning = Se intentó guardar un mapa post-init sin forzar el guardado.
cmd-savemap-attempt = Intentando guardar el mapa {$mapId} en {$path}.
cmd-savemap-success = Mapa guardado exitosamente.
cmd-savemap-error = ¡No se pudo guardar el mapa! Consulta el log del servidor para más detalles.
cmd-hint-savemap-id = <MapID>
cmd-hint-savemap-path = <Ruta>
cmd-hint-savemap-force = [bool]

cmd-loadmap-desc = Carga un mapa desde el disco al juego.
cmd-loadmap-help = loadmap <MapID> <Ruta> [x] [y] [rotación] [consistentUids]
cmd-loadmap-nullspace = No puedes cargar en el mapa 0.
cmd-loadmap-exists = El mapa {$mapId} ya existe.
cmd-loadmap-success = El mapa {$mapId} ha sido cargado desde {$path}.
cmd-loadmap-error = Ocurrió un error al cargar el mapa desde {$path}.
cmd-hint-loadmap-x-position = [posición-x]
cmd-hint-loadmap-y-position = [posición-y]
cmd-hint-loadmap-rotation = [rotación]
cmd-hint-loadmap-uids = [float]

cmd-hint-savebp-id = <EntityID de Grilla>

## comando 'flushcookies'

cmd-flushcookies-desc = Vacía el almacenamiento de cookies CEF al disco
cmd-flushcookies-help = Esto asegura que las cookies se guarden correctamente en disco en caso de cierres no limpios.
    Ten en cuenta que la operación real es asíncrona.

cmd-ldrsc-desc = Pre-cachea un recurso.
cmd-ldrsc-help = Uso: ldrsc <ruta> <tipo>

cmd-rldrsc-desc = Recarga un recurso.
cmd-rldrsc-help = Uso: rldrsc <ruta> <tipo>

cmd-gridtc-desc = Obtiene el conteo de baldosas de una grilla.
cmd-gridtc-help = Uso: gridtc <gridId>


# Comandos del lado del cliente
cmd-guidump-desc = Vuelca el árbol GUI a /guidump.txt en datos de usuario.
cmd-guidump-help = Uso: guidump

cmd-uitest-desc = Abre una ventana ficticia de prueba de UI
cmd-uitest-help = Uso: uitest

## comando 'uitest2'
cmd-uitest2-desc = Abre una ventana de OS para probar controles de UI
cmd-uitest2-help = Uso: uitest2 <pestaña>
cmd-uitest2-arg-tab = <pestaña>
cmd-uitest2-error-args = Se esperaba como máximo un argumento
cmd-uitest2-error-tab = Pestaña inválida: '{$value}'
cmd-uitest2-title = UITest2


cmd-setclipboard-desc = Establece el portapapeles del sistema
cmd-setclipboard-help = Uso: setclipboard <texto>

cmd-getclipboard-desc = Obtiene el portapapeles del sistema
cmd-getclipboard-help = Uso: getclipboard

cmd-togglelight-desc = Activa/desactiva el renderizado de luces.
cmd-togglelight-help = Uso: togglelight

cmd-togglefov-desc = Activa/desactiva el campo de visión del cliente.
cmd-togglefov-help = Uso: togglefov

cmd-togglehardfov-desc = Activa/desactiva el campo de visión estricto del cliente. (para depuración de space-station-14#2353)
cmd-togglehardfov-help = Uso: togglehardfov

cmd-toggleshadows-desc = Activa/desactiva el renderizado de sombras.
cmd-toggleshadows-help = Uso: toggleshadows

cmd-togglelightbuf-desc = Activa/desactiva el renderizado de iluminación. Incluye sombras pero no FOV.
cmd-togglelightbuf-help = Uso: togglelightbuf

cmd-chunkinfo-desc = Obtiene información sobre un chunk bajo el cursor del ratón.
cmd-chunkinfo-help = Uso: chunkinfo

cmd-rldshader-desc = Recarga todos los shaders.
cmd-rldshader-help = Uso: rldshader

cmd-cldbglyr-desc = Activa/desactiva capas de depuración de FOV y luz.
cmd-cldbglyr-help= Uso: cldbglyr <capa>: Activa/desactiva <capa>
    cldbglyr: Desactiva todas las capas

cmd-key-info-desc = Muestra información de tecla para una tecla.
cmd-key-info-help = Uso: keyinfo <Tecla>

## comando 'bind'
cmd-bind-desc = Vincula una combinación de teclas a un comando de entrada.
cmd-bind-help = Uso: bind { cmd-bind-arg-key } { cmd-bind-arg-mode } { cmd-bind-arg-command }
    Ten en cuenta que esto NO guarda automáticamente las vinculaciones.
    Usa el comando 'svbind' para guardar la configuración de vinculaciones.

cmd-bind-arg-key = <NombreTecla>
cmd-bind-arg-mode = <ModoVinculación>
cmd-bind-arg-command = <ComandoEntrada>

cmd-net-draw-interp-desc = Activa/desactiva el dibujo de depuración de la interpolación de red.
cmd-net-draw-interp-help = Uso: net_draw_interp

cmd-net-watch-ent-desc = Vuelca todas las actualizaciones de red de un EntityId a la consola.
cmd-net-watch-ent-help = Uso: net_watchent <0|EntityUid>

cmd-net-refresh-desc = Solicita un estado completo del servidor.
cmd-net-refresh-help = Uso: net_refresh

cmd-net-entity-report-desc = Activa/desactiva el panel de informe de entidades de red.
cmd-net-entity-report-help = Uso: net_entityreport

cmd-fill-desc = Llena la consola para depuración.
cmd-fill-help = Llena la consola con texto sin sentido para depuración.

cmd-cls-desc = Limpia la consola.
cmd-cls-help = Limpia la consola de depuración de todos los mensajes.

cmd-sendgarbage-desc = Envía basura al servidor.
cmd-sendgarbage-help = El servidor responderá con 'no u'

cmd-loadgrid-desc = Carga una grilla desde un archivo en un mapa existente.
cmd-loadgrid-help = loadgrid <MapID> <Ruta> [x y] [rotación] [storeUids]

cmd-loc-desc = Imprime la ubicación absoluta de la entidad del jugador en la consola.
cmd-loc-help = loc

cmd-tpgrid-desc = Teletransporta una grilla a una nueva ubicación.
cmd-tpgrid-help = tpgrid <gridId> <X> <Y> [<MapId>]

cmd-rmgrid-desc = Elimina una grilla de un mapa. No puedes eliminar la grilla predeterminada.
cmd-rmgrid-help = rmgrid <gridId>

cmd-mapinit-desc = Ejecuta map init en un mapa.
cmd-mapinit-help = mapinit <mapID>

cmd-lsmap-desc = Lista los mapas.
cmd-lsmap-help = lsmap

cmd-lsgrid-desc = Lista las grillas.
cmd-lsgrid-help = lsgrid

cmd-addmap-desc = Añade un nuevo mapa vacío a la ronda. Si el mapID ya existe, este comando no hace nada.
cmd-addmap-help = addmap <mapID> [pre-init]

cmd-rmmap-desc = Elimina un mapa del mundo. No puedes eliminar el espacio nulo.
cmd-rmmap-help = rmmap <mapId>

cmd-savegrid-desc = Serializa una grilla al disco.
cmd-savegrid-help = savegrid <gridID> <Ruta>

cmd-testbed-desc = Carga un banco de pruebas de física en el mapa especificado.
cmd-testbed-help = testbed <mapid> <test>

## comando 'addcomp'
cmd-addcomp-desc = Añade un componente a una entidad.
cmd-addcomp-help = addcomp <uid> <nombreComponente>
cmd-addcompc-desc = Añade un componente a una entidad en el cliente.
cmd-addcompc-help = addcompc <uid> <nombreComponente>

## comando 'rmcomp'
cmd-rmcomp-desc = Elimina un componente de una entidad.
cmd-rmcomp-help = rmcomp <uid> <nombreComponente>
cmd-rmcompc-desc = Elimina un componente de una entidad en el cliente.
cmd-rmcompc-help = rmcomp <uid> <nombreComponente>

## comando 'addview'
cmd-addview-desc = Permite suscribirse a la vista de una entidad para fines de depuración.
cmd-addview-help = addview <entityUid>
cmd-addviewc-desc = Permite suscribirse a la vista de una entidad para fines de depuración.
cmd-addviewc-help = addview <entityUid>

## comando 'removeview'
cmd-removeview-desc = Permite cancelar la suscripción a la vista de una entidad para fines de depuración.
cmd-removeview-help = removeview <entityUid>

## comando 'loglevel'
cmd-loglevel-desc = Cambia el nivel de log para un sawmill proporcionado.
cmd-loglevel-help = Uso: loglevel <sawmill> <nivel>
      sawmill: Una etiqueta que precede los mensajes de log. Es para el que estás configurando el nivel.
      nivel: El nivel de log. Debe coincidir con uno de los valores del enum LogLevel.

cmd-testlog-desc = Escribe un log de prueba en un sawmill.
cmd-testlog-help = Uso: testlog <sawmill> <nivel> <mensaje>
    sawmill: Una etiqueta que precede el mensaje registrado.
    nivel: El nivel de log. Debe coincidir con uno de los valores del enum LogLevel.
    mensaje: El mensaje a registrar. Enciérralo en comillas dobles si quieres usar espacios.

## comando 'vv'
cmd-vv-desc = Abre Ver Variables.
cmd-vv-help = Uso: vv <ID de entidad|nombre de interfaz IoC|nombre de interfaz SIoC>

## comando 'showvelocities'
cmd-showvelocities-desc = Muestra tus velocidades angular y lineal.
cmd-showvelocities-help = Uso: showvelocities

## comando 'setinputcontext'
cmd-setinputcontext-desc = Establece el contexto de entrada activo.
cmd-setinputcontext-help = Uso: setinputcontext <contexto>

## comando 'forall'
cmd-forall-desc = Ejecuta un comando sobre todas las entidades con un componente dado.
cmd-forall-help = Uso: forall <consulta bql> do <comando...>

## comando 'delete'
cmd-delete-desc = Elimina la entidad con el ID especificado.
cmd-delete-help = delete <UID de entidad>

# Comandos del sistema
cmd-showtime-desc = Muestra la hora del servidor.
cmd-showtime-help = showtime

cmd-restart-desc = Reinicia el servidor de forma elegante (no solo la ronda).
cmd-restart-help = restart

cmd-shutdown-desc = Apaga el servidor de forma elegante.
cmd-shutdown-help = shutdown

cmd-saveconfig-desc = Guarda la configuración del servidor en el archivo de configuración.
cmd-saveconfig-help = saveconfig

cmd-netaudit-desc = Imprime información sobre la seguridad de NetMsg.
cmd-netaudit-help = netaudit

# Comandos de jugador
cmd-tp-desc = Teletransporta a un jugador a cualquier ubicación en la ronda.
cmd-tp-help = tp <x> <y> [<mapID>]

cmd-tpto-desc = Teletransporta al jugador actual o a los jugadores/entidades especificados a la ubicación del primer jugador/entidad.
cmd-tpto-help = tpto <usuario|uid> [usuario|NetEntity]...
cmd-tpto-destination-hint = destino (NetEntity o nombre de usuario)
cmd-tpto-victim-hint = entidad a teletransportar (NetEntity o nombre de usuario)
cmd-tpto-parse-error = No se puede resolver la entidad o jugador: {$str}

cmd-listplayers-desc = Lista todos los jugadores conectados actualmente.
cmd-listplayers-help = listplayers

cmd-kick-desc = Expulsa a un jugador conectado del servidor, desconectándolo.
cmd-kick-help = kick <PlayerIndex> [<Razón>]

# Comando spin
cmd-spin-desc = Hace que una entidad gire. La entidad predeterminada es el padre del jugador asociado.
cmd-spin-help = spin velocidad [arrastre] [entityUid]

# Comando de localización
cmd-rldloc-desc = Recarga la localización (cliente y servidor).
cmd-rldloc-help = Uso: rldloc

# Controles de depuración de entidades
cmd-spawn-desc = Genera una entidad con un tipo específico.
cmd-spawn-help = spawn <prototipo> O spawn <prototipo> <ID entidad relativa> O spawn <prototipo> <x> <y>
cmd-cspawn-desc = Genera una entidad del lado del cliente con un tipo específico a tus pies.
cmd-cspawn-help = cspawn <tipo de entidad>

cmd-dumpentities-desc = Vuelca la lista de entidades.
cmd-dumpentities-help = Vuelca la lista de entidades con UIDs y prototipos.

cmd-getcomponentregistration-desc = Obtiene información del registro de un componente.
cmd-getcomponentregistration-help = Uso: getcomponentregistration <nombreComponente>

cmd-showrays-desc = Activa/desactiva el dibujo de depuración de rayos de física. Se debe proporcionar un entero para <vidaDelRayo>.
cmd-showrays-help = Uso: showrays <vidaDelRayo>

cmd-disconnect-desc = Se desconecta inmediatamente del servidor y regresa al menú principal.
cmd-disconnect-help = Uso: disconnect

cmd-entfo-desc = Muestra diagnósticos detallados para una entidad.
cmd-entfo-help = Uso: entfo <entityuid>
    El UID de entidad puede tener el prefijo 'c' para convertirlo en un UID de entidad de cliente.

cmd-fuck-desc = Lanza una excepción
cmd-fuck-help = Uso: fuck

cmd-showpos-desc = Muestra la posición de todas las entidades en la pantalla.
cmd-showpos-help = Uso: showpos

cmd-showrot-desc = Muestra la rotación de todas las entidades en la pantalla.
cmd-showrot-help = Uso: showrot

cmd-showvel-desc = Muestra la velocidad local de todas las entidades en la pantalla.
cmd-showvel-help = Uso: showvel

cmd-showangvel-desc = Muestra la velocidad angular de todas las entidades en la pantalla.
cmd-showangvel-help = Uso: showangvel

cmd-sggcell-desc = Lista entidades en una celda de snap grid.
cmd-sggcell-help = Uso: sggcell <gridID> <vector2i>\nEse parámetro vector2i tiene la forma x<int>,y<int>.

cmd-overrideplayername-desc = Cambia el nombre usado al intentar conectarse al servidor.
cmd-overrideplayername-help = Uso: overrideplayername <nombre>

cmd-showanchored-desc = Muestra entidades ancladas en una baldosa particular
cmd-showanchored-help = Uso: showanchored

cmd-dmetamem-desc = Vuelca los miembros de un tipo en un formato adecuado para el archivo de configuración de sandbox.
cmd-dmetamem-help = Uso: dmetamem <tipo>

cmd-launchauth-desc = Carga tokens de autenticación desde datos del launcher para ayudar en pruebas de servidores en vivo.
cmd-launchauth-help = Uso: launchauth <nombre de cuenta>

cmd-lightbb-desc = Activa/desactiva la visualización de cajas delimitadoras de luz.
cmd-lightbb-help = Uso: lightbb

cmd-monitorinfo-desc = Información de monitores
cmd-monitorinfo-help = Uso: monitorinfo <id>

cmd-setmonitor-desc = Establecer monitor
cmd-setmonitor-help = Uso: setmonitor <id>

cmd-physics-desc = Muestra una capa de depuración de física. El argumento proporcionado especifica la capa.
cmd-physics-help = Uso: physics <aabbs / com / contactnormals / contactpoints / distance / joints / shapeinfo / shapes>

cmd-hardquit-desc = Cierra el cliente del juego instantáneamente.
cmd-hardquit-help = Cierra el cliente del juego al instante, sin dejar rastro. Sin despedirse del servidor.

cmd-quit-desc = Cierra el cliente del juego de forma elegante.
cmd-quit-help = Cierra correctamente el cliente del juego, notificando al servidor conectado y demás.

cmd-csi-desc = Abre una consola interactiva de C#.
cmd-csi-help = Uso: csi

cmd-scsi-desc = Abre una consola interactiva de C# en el servidor.
cmd-scsi-help = Uso: scsi

cmd-watch-desc = Abre una ventana de observación de variables.
cmd-watch-help = Uso: watch

cmd-showspritebb-desc = Activa/desactiva la visualización de límites de sprites
cmd-showspritebb-help = Uso: showspritebb

cmd-togglelookup-desc = Muestra / oculta los límites de entitylookup mediante una capa.
cmd-togglelookup-help = Uso: togglelookup

cmd-net_entityreport-desc = Activa/desactiva el panel de informe de entidades de red.
cmd-net_entityreport-help = Uso: net_entityreport

cmd-net_refresh-desc = Solicita un estado completo del servidor.
cmd-net_refresh-help = Uso: net_refresh

cmd-net_graph-desc = Activa/desactiva el panel de estadísticas de red.
cmd-net_graph-help = Uso: net_graph

cmd-net_watchent-desc = Vuelca todas las actualizaciones de red de un EntityId a la consola.
cmd-net_watchent-help = Uso: net_watchent <0|EntityUid>

cmd-net_draw_interp-desc = Activa/desactiva el dibujo de depuración de la interpolación de red.
cmd-net_draw_interp-help = Uso: net_draw_interp <0|EntityUid>

cmd-vram-desc = Muestra estadísticas de uso de memoria de video del juego.
cmd-vram-help = Uso: vram

cmd-showislands-desc = Muestra los cuerpos físicos actuales involucrados en cada isla de física.
cmd-showislands-help = Uso: showislands

cmd-showgridnodes-desc = Muestra los nodos para propósitos de división de grillas.
cmd-showgridnodes-help = Uso: showgridnodes

cmd-profsnap-desc = Toma una instantánea de perfilado.
cmd-profsnap-help = Uso: profsnap

cmd-devwindow-desc = Ventana de Desarrollo
cmd-devwindow-help = Uso: devwindow

cmd-scene-desc = Cambia inmediatamente la escena/estado de la UI.
cmd-scene-help = Uso: scene <nombreClase>

cmd-szr_stats-desc = Reporta estadísticas del serializador.
cmd-szr_stats-help = Uso: szr_stats

cmd-hwid-desc = Devuelve el HWID actual (ID de Hardware).
cmd-hwid-help = Uso: hwid

cmd-vvread-desc = Obtiene el valor de una ruta usando VV (Ver Variables).
cmd-vvread-help = Uso: vvread <ruta>

cmd-vvwrite-desc = Modifica el valor de una ruta usando VV (Ver Variables).
cmd-vvwrite-help = Uso: vvwrite <ruta>

cmd-vvinvoke-desc = Invoca/Llama una ruta con argumentos usando VV.
cmd-vvinvoke-help = Uso: vvinvoke <ruta> [argumentos...]

cmd-dump_dependency_injectors-desc = Vuelca la caché de inyectores de dependencia del IoCManager.
cmd-dump_dependency_injectors-help = Uso: dump_dependency_injectors
cmd-dump_dependency_injectors-total-count = Conteo total: { $total }

cmd-dump_netserializer_type_map-desc = Vuelca el mapa de tipos y hash del serializador de NetSerializer.
cmd-dump_netserializer_type_map-help = Uso: dump_netserializer_type_map

cmd-hub_advertise_now-desc = Anuncia inmediatamente al servidor hub maestro
cmd-hub_advertise_now-help = Uso: hub_advertise_now

cmd-echo-desc = Repite los argumentos en la consola
cmd-echo-help = Uso: echo "<mensaje>"

## comando 'vfs_ls'
cmd-vfs_ls-desc = Lista el contenido de un directorio en el VFS.
cmd-vfs_ls-help = Uso: vfs_list <ruta>
    Ejemplo:
    vfs_list /Assemblies

cmd-vfs_ls-err-args = Se necesita exactamente 1 argumento.
cmd-vfs_ls-hint-path = <ruta>

cmd-reloadtiletextures-desc = Recarga el atlas de texturas de baldosas para permitir la recarga en caliente de sprites de baldosas
cmd-reloadtiletextures-help = Uso: reloadtiletextures

cmd-audio_length-desc = Muestra la duración de un archivo de audio
cmd-audio_length-help = Uso: audio_length { cmd-audio_length-arg-file-name }
cmd-audio_length-arg-file-name = <nombre de archivo>

## PVS
cmd-pvs-override-info-desc = Imprime información sobre cualquier anulación PVS asociada a una entidad.
cmd-pvs-override-info-empty = La entidad {$nuid} no tiene anulaciones PVS.
cmd-pvs-override-info-global = La entidad {$nuid} tiene una anulación global.
cmd-pvs-override-info-clients = La entidad {$nuid} tiene una anulación de sesión para {$clients}.

cmd-localization_set_culture-desc = Establece DefaultCulture para el LocalizationManager del cliente
cmd-localization_set_culture-help = Uso: localization_set_culture <nombreCultura>
cmd-localization_set_culture-culture-name = <nombreCultura>
cmd-localization_set_culture-changed = Localización cambiada a { $code } ({ $nativeName } / { $englishName })
