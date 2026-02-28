# SPDX-FileCopyrightText: 2023 LankLTE <135308300+LankLTE@users.noreply.github.com>
# SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
# SPDX-FileCopyrightText: 2023 Sailor <109166122+Equivocateur@users.noreply.github.com>
# SPDX-FileCopyrightText: 2023 mhamster <81412348+mhamsterr@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 Aidenkrz <aiden@djkraz.com>
# SPDX-FileCopyrightText: 2024 Eris <eris@erisws.com>
# SPDX-FileCopyrightText: 2024 Flesh <62557990+PolterTzi@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 Gotimanga <127038462+Gotimanga@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 Steve <marlumpy@gmail.com>
# SPDX-FileCopyrightText: 2024 Zonespace <41448081+Zonespace27@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 alex-georgeff <54858069+taurie@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 marc-pelletier <113944176+marc-pelletier@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 2025 SX-7 <92227810+SX-7@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later

-create-3rd-person =
    { $chance ->
        [1] Crea
        *[other] crear
    }

-cause-3rd-person =
    { $chance ->
        [1] Causa
        *[other] causar
    }

-satiate-3rd-person =
    { $chance ->
        [1] Sacia
        *[other] saciar
    }

reagent-effect-guidebook-create-entity-reaction-effect =
    { $chance ->
        [1] Crea
        *[other] crear
    } { $amount ->
        [1] {INDEFINITE($entname)}
        *[other] {$amount} {MAKEPLURAL($entname)}
    }

reagent-effect-guidebook-explosion-reaction-effect =
    { $chance ->
        [1] Causa
        *[other] causar
    } una explosión

reagent-effect-guidebook-emp-reaction-effect =
    { $chance ->
        [1] Causa
        *[other] causar
    } un pulso electromagnético

reagent-effect-guidebook-flash-reaction-effect =
    { $chance ->
        [1] Causa
        *[other] causar
    } un destello cegador

reagent-effect-guidebook-foam-area-reaction-effect =
    { $chance ->
        [1] Crea
        *[other] crear
    } grandes cantidades de espuma

reagent-effect-guidebook-smoke-area-reaction-effect =
    { $chance ->
        [1] Crea
        *[other] crear
    } grandes cantidades de humo

reagent-effect-guidebook-satiate-thirst =
    { $chance ->
        [1] Sacia
        *[other] saciar
    } { $relative ->
        [1] la sed de forma normal
        *[other] la sed a {NATURALFIXED($relative, 3)}x la tasa normal
    }

reagent-effect-guidebook-satiate-hunger =
    { $chance ->
        [1] Sacia
        *[other] saciar
    } { $relative ->
        [1] el hambre de forma normal
        *[other] el hambre a {NATURALFIXED($relative, 3)}x la tasa normal
    }

reagent-effect-guidebook-health-change =
    { $chance ->
        [1] { $healsordeals ->
                [heals] Cura
                [deals] Inflige
                *[both] Modifica la salud en
             }
        *[other] { $healsordeals ->
                    [heals] curar
                    [deals] infligir
                    *[both] modificar la salud en
                 }
    } { $changes }

reagent-effect-guidebook-even-health-change =
    { $chance ->
        [1] { $healsordeals ->
            [heals] Cura uniformemente
            [deals] Inflige uniformemente
            *[both] Modifica uniformemente la salud en
        }
        *[other] { $healsordeals ->
            [heals] curar uniformemente
            [deals] infligir uniformemente
            *[both] modificar uniformemente la salud en
        }
    } { $changes }



reagent-effect-guidebook-status-effect =
    { $type ->
        [add]   { $chance ->
                    [1] Causa
                    *[other] causar
                } {LOC($key)} durante al menos {NATURALFIXED($time, 3)} {MANY("second", $time)} con acumulación
        *[set]  { $chance ->
                    [1] Causa
                    *[other] causar
                } {LOC($key)} durante al menos {NATURALFIXED($time, 3)} {MANY("second", $time)} sin acumulación
        [remove]{ $chance ->
                    [1] Elimina
                    *[other] eliminar
                } {NATURALFIXED($time, 3)} {MANY("second", $time)} de {LOC($key)}
    }

reagent-effect-guidebook-set-solution-temperature-effect =
    { $chance ->
        [1] Establece
        *[other] establecer
    } la temperatura de la solución a exactamente {NATURALFIXED($temperature, 2)}k

reagent-effect-guidebook-adjust-solution-temperature-effect =
    { $chance ->
        [1] { $deltasign ->
                [1] Añade
                *[-1] Elimina
            }
        *[other]
            { $deltasign ->
                [1] añadir
                *[-1] eliminar
            }
    } calor { $deltasign ->
                [1] a
                *[-1] de
           } la solución hasta alcanzar { $deltasign ->
                [1] como máximo {NATURALFIXED($maxtemp, 2)}k
                *[-1] al menos {NATURALFIXED($mintemp, 2)}k
            }

reagent-effect-guidebook-adjust-reagent-reagent =
    { $chance ->
        [1] { $deltasign ->
                [1] Añade
                *[-1] Elimina
            }
        *[other]
            { $deltasign ->
                [1] añadir
                *[-1] eliminar
            }
    } {NATURALFIXED($amount, 2)}u de {$reagent} { $deltasign ->
        [1] a
        *[-1] de
    } la solución

reagent-effect-guidebook-adjust-reagent-group =
    { $chance ->
        [1] { $deltasign ->
                [1] Añade
                *[-1] Elimina
            }
        *[other]
            { $deltasign ->
                [1] añadir
                *[-1] eliminar
            }
    } {NATURALFIXED($amount, 2)}u de reactivos del grupo {$group} { $deltasign ->
            [1] a
            *[-1] de
        } la solución

reagent-effect-guidebook-adjust-temperature =
    { $chance ->
        [1] { $deltasign ->
                [1] Añade
                *[-1] Elimina
            }
        *[other]
            { $deltasign ->
                [1] añadir
                *[-1] eliminar
            }
    } {POWERJOULES($amount)} de calor { $deltasign ->
            [1] al
            *[-1] del
        } cuerpo en el que se encuentra

reagent-effect-guidebook-chem-cause-disease =
    { $chance ->
        [1] Causa
        *[other] causar
    } la enfermedad { $disease }

reagent-effect-guidebook-chem-cause-random-disease =
    { $chance ->
        [1] Causa
        *[other] causar
    } las enfermedades { $diseases }

reagent-effect-guidebook-jittering =
    { $chance ->
        [1] Causa
        *[other] causar
    } temblores

reagent-effect-guidebook-chem-clean-bloodstream =
    { $chance ->
        [1] Limpia
        *[other] limpiar
    } el torrente sanguíneo de otros químicos

reagent-effect-guidebook-cure-disease =
    { $chance ->
        [1] Cura
        *[other] curar
    } enfermedades

reagent-effect-guidebook-cure-eye-damage =
    { $chance ->
        [1] { $deltasign ->
                [1] Inflige
                *[-1] Cura
            }
        *[other]
            { $deltasign ->
                [1] infligir
                *[-1] curar
            }
    } daño ocular

reagent-effect-guidebook-chem-vomit =
    { $chance ->
        [1] Causa
        *[other] causar
    } vómitos

reagent-effect-guidebook-create-gas =
    { $chance ->
        [1] Crea
        *[other] crear
    } { $moles } { $moles ->
        [1] mol
        *[other] moles
    } de { $gas }

reagent-effect-guidebook-drunk =
    { $chance ->
        [1] Causa
        *[other] causar
    } embriaguez

reagent-effect-guidebook-electrocute =
    { $chance ->
        [1] Electrocuta
        *[other] electrocutar
    } al metabolizador durante {NATURALFIXED($time, 3)} {MANY("second", $time)}

reagent-effect-guidebook-emote =
    { $chance ->
        [1] Obliga
        *[other] obligar
    } al metabolizador a [bold][color=white]{$emote}[/color][/bold]

reagent-effect-guidebook-extinguish-reaction =
    { $chance ->
        [1] Extingue
        *[other] extinguir
    } el fuego

reagent-effect-guidebook-flammable-reaction =
    { $chance ->
        [1] Aumenta
        *[other] aumentar
    } la inflamabilidad

reagent-effect-guidebook-ignite =
    { $chance ->
        [1] Enciende
        *[other] encender
    } al metabolizador

reagent-effect-guidebook-make-sentient =
    { $chance ->
        [1] Hace
        *[other] hacer
    } sintiente al metabolizador

reagent-effect-guidebook-make-polymorph =
    { $chance ->
        [1] Transforma
        *[other] transformar
    } al metabolizador en un { $entityname }

reagent-effect-guidebook-modify-bleed-amount =
    { $chance ->
        [1] { $deltasign ->
                [1] Induce
                *[-1] Reduce
            }
        *[other] { $deltasign ->
                    [1] inducir
                    *[-1] reducir
                 }
    } sangrado

reagent-effect-guidebook-modify-blood-level =
    { $chance ->
        [1] { $deltasign ->
                [1] Aumenta
                *[-1] Disminuye
            }
        *[other] { $deltasign ->
                    [1] aumentar
                    *[-1] disminuir
                 }
    } el nivel de sangre

reagent-effect-guidebook-paralyze =
    { $chance ->
        [1] Paraliza
        *[other] paralizar
    } al metabolizador durante al menos {NATURALFIXED($time, 3)} {MANY("second", $time)}

reagent-effect-guidebook-movespeed-modifier =
    { $chance ->
        [1] Modifica
        *[other] modificar
    } la velocidad de movimiento en {NATURALFIXED($walkspeed, 3)}x durante al menos {NATURALFIXED($time, 3)} {MANY("second", $time)}

reagent-effect-guidebook-reset-narcolepsy =
    { $chance ->
        [1] Alivia
        *[other] aliviar
    } temporalmente la narcolepsia

reagent-effect-guidebook-wash-cream-pie-reaction =
    { $chance ->
        [1] Limpia
        *[other] limpiar
    } el pastelazo de la cara

reagent-effect-guidebook-cure-zombie-infection =
    { $chance ->
        [1] Cura
        *[other] curar
    } una infección zombi en curso

reagent-effect-guidebook-cause-zombie-infection =
    { $chance ->
        [1] Contagia
        *[other] contagiar
    } la infección zombi a un individuo

reagent-effect-guidebook-innoculate-zombie-infection =
    { $chance ->
        [1] Cura
        *[other] curar
    } una infección zombi en curso, y proporciona inmunidad a futuras infecciones

reagent-effect-guidebook-reduce-rotting =
    { $chance ->
        [1] Regenera
        *[other] regenerar
    } {NATURALFIXED($time, 3)} {MANY("second", $time)} de putrefacción

reagent-effect-guidebook-area-reaction =
    { $chance ->
        [1] Causa
        *[other] causar
    } una reacción de humo o espuma durante {NATURALFIXED($duration, 3)} {MANY("second", $duration)}

reagent-effect-guidebook-add-to-solution-reaction =
    { $chance ->
        [1] Causa
        *[other] causar
    } que los químicos aplicados a un objeto se añadan a su contenedor de solución interno

reagent-effect-guidebook-artifact-unlock =
    { $chance ->
        [1] Ayuda
        *[other] ayudar
        } a desbloquear un artefacto alienígena.

reagent-effect-guidebook-artifact-durability-restore =
    Restaura {$restored} de durabilidad en nodos activos de artefactos alienígenas.

reagent-effect-guidebook-plant-attribute =
    { $chance ->
        [1] Ajusta
        *[other] ajustar
    } {$attribute} en [color={$colorName}]{$amount}[/color]

reagent-effect-guidebook-plant-cryoxadone =
    { $chance ->
        [1] Rejuvenece
        *[other] rejuvenecer
    } la planta, dependiendo de la edad de la planta y el tiempo de crecimiento

reagent-effect-guidebook-plant-phalanximine =
    { $chance ->
        [1] Restaura
        *[other] restaurar
    } la viabilidad de una planta no viable por una mutación

reagent-effect-guidebook-plant-diethylamine =
    { $chance ->
        [1] Aumenta
        *[other] aumentar
    } la esperanza de vida y/o la salud base de la planta con un 10% de probabilidad para cada una

reagent-effect-guidebook-plant-robust-harvest =
    { $chance ->
        [1] Aumenta
        *[other] aumentar
    } la potencia de la planta en {$increase} hasta un máximo de {$limit}. Causa que la planta pierda sus semillas una vez que la potencia alcance {$seedlesstreshold}. Intentar añadir potencia por encima de {$limit} puede causar una disminución del rendimiento con un 10% de probabilidad

reagent-effect-guidebook-plant-seeds-add =
    { $chance ->
        [1] Restaura las
        *[other] restaurar las
    } semillas de la planta

reagent-effect-guidebook-plant-seeds-remove =
    { $chance ->
        [1] Elimina las
        *[other] eliminar las
    } semillas de la planta

reagent-effect-guidebook-add-to-chemicals =
    { $chance ->
        [1] { $deltasign ->
                [1] Añade
                *[-1] Elimina
            }
        *[other]
            { $deltasign ->
                [1] añadir
                *[-1] eliminar
            }
    } {NATURALFIXED($amount, 2)}u de {$reagent} { $deltasign ->
        [1] a
        *[-1] de
    } la solución
