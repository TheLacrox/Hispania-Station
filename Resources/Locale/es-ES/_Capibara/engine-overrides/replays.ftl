# Comandos de Reproducción

cmd-replay-play-desc = Reanudar la reproducción de la repetición.
cmd-replay-play-help = replay_play

cmd-replay-pause-desc = Pausar la reproducción de la repetición.
cmd-replay-pause-help = replay_pause

cmd-replay-toggle-desc = Reanudar o pausar la reproducción de la repetición.
cmd-replay-toggle-help = replay_toggle

cmd-replay-stop-desc = Detener y descargar una repetición.
cmd-replay-stop-help = replay_stop

cmd-replay-load-desc = Cargar e iniciar una repetición.
cmd-replay-load-help = replay_load <carpeta de repetición>
cmd-replay-load-hint = Carpeta de repetición

cmd-replay-skip-desc = Saltar hacia adelante o atrás en el tiempo.
cmd-replay-skip-help = replay_skip <tick o duración>
cmd-replay-skip-hint = Ticks o duración (HH:MM:SS).

cmd-replay-set-time-desc = Saltar a un momento específico.
cmd-replay-set-time-help = replay_set <tick o tiempo>
cmd-replay-set-time-hint = Tick o duración (HH:MM:SS), comenzando desde

cmd-replay-error-time = "{$time}" no es un entero ni una duración válida.
cmd-replay-error-args = Número incorrecto de argumentos.
cmd-replay-error-no-replay = No se está reproduciendo ninguna repetición actualmente.
cmd-replay-error-already-loaded = Ya hay una repetición cargada.
cmd-replay-error-run-level = No puedes cargar una repetición mientras estás conectado a un servidor.

# Comandos de Grabación

cmd-replay-recording-start-desc = Inicia una grabación de repetición, opcionalmente con un límite de tiempo.
cmd-replay-recording-start-help = Uso: replay_recording_start [nombre] [sobrescribir] [límite de tiempo]
cmd-replay-recording-start-success = Se inició la grabación de una repetición.
cmd-replay-recording-start-already-recording = Ya se está grabando una repetición.
cmd-replay-recording-start-error = Ocurrió un error al intentar iniciar la grabación.
cmd-replay-recording-start-hint-time = [límite de tiempo (minutos)]
cmd-replay-recording-start-hint-name = [nombre]
cmd-replay-recording-start-hint-overwrite = [sobrescribir (bool)]

cmd-replay-recording-stop-desc = Detiene una grabación de repetición.
cmd-replay-recording-stop-help = Uso: replay_recording_stop
cmd-replay-recording-stop-success = Se detuvo la grabación de la repetición.
cmd-replay-recording-stop-not-recording = No se está grabando ninguna repetición actualmente.

cmd-replay-recording-stats-desc = Muestra información sobre la grabación de repetición actual.
cmd-replay-recording-stats-help = Uso: replay_recording_stats
cmd-replay-recording-stats-result = Duración: {$time} min, Ticks: {$ticks}, Tamaño: {$size} MB, velocidad: {$rate} MB/min.


# Interfaz de Control de Tiempo
replay-time-box-scrubbing-label = Desplazamiento Dinámico
replay-time-box-replay-time-label = Tiempo de Grabación: {$current} / {$end}  ({$percentage}%)
replay-time-box-server-time-label = Tiempo del Servidor: {$current} / {$end}
replay-time-box-index-label = Índice: {$current} / {$total}
replay-time-box-tick-label = Tick: {$current} / {$total}
