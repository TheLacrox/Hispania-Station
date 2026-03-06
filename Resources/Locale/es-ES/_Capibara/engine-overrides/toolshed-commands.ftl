command-help-usage =
    Uso:
command-help-invertible =
    El comportamiento de este comando puede invertirse usando el prefijo "not".
command-description-tpto =
    Teletransporta las entidades dadas a una entidad objetivo.
command-description-player-list =
    Devuelve una lista de todas las sesiones de jugador.
command-description-player-self =
    Devuelve la sesión del jugador actual.
command-description-player-imm =
    Devuelve la sesión asociada al jugador proporcionado como argumento.
command-description-player-entity =
    Devuelve las entidades de las sesiones de entrada.
command-description-self =
    Devuelve la entidad actualmente asociada.
command-description-physics-velocity =
    Devuelve la velocidad de las entidades de entrada.
command-description-physics-angular-velocity =
    Devuelve la velocidad angular de las entidades de entrada.
command-description-buildinfo =
    Proporciona información sobre la compilación del juego.
command-description-cmd-list =
    Devuelve una lista de todos los comandos, para este lado.
command-description-explain =
    Explica la expresión dada, proporcionando descripciones y firmas de comandos. Solo funciona para expresiones válidas, no puede explicar comandos que no logra analizar.
command-description-search =
    Busca en la entrada el valor proporcionado.
command-description-stopwatch =
    Mide el tiempo de ejecución de la expresión dada.
command-description-types-consumers =
    Proporciona todos los comandos que pueden consumir el tipo dado.
command-description-types-tree =
    Herramienta de depuración para devolver todos los tipos a los que el intérprete de comandos puede convertir la entrada.
command-description-types-gettype =
    Devuelve el tipo de la entrada.
command-description-types-fullname =
    Devuelve el nombre completo del tipo de entrada según CoreCLR.
command-description-as =
    Convierte la entrada al tipo dado.
    Efectivamente una pista de tipo si conoces el tipo pero el intérprete no.
command-description-count =
    Cuenta la cantidad de entradas, devolviendo un entero.
command-description-map =
    Mapea la entrada sobre el bloque dado.
command-description-select =
    Selecciona N objetos o N% de objetos de la entrada.
    Adicionalmente se puede invertir este comando con not para seleccionar todo excepto N objetos.
command-description-comp =
    Devuelve el componente dado de las entidades de entrada, descartando entidades sin ese componente.
command-description-delete =
    Elimina las entidades de entrada.
command-description-ent =
    Devuelve el ID de entidad proporcionado.
command-description-entities =
    Devuelve todas las entidades en el servidor.
command-description-paused =
    Filtra las entidades de entrada según si están pausadas o no.
command-description-with =
    Filtra las entidades de entrada según si tienen o no el componente dado.
command-description-fuck =
    Lanza una excepción.
command-description-ecscomp-listty =
    Lista todos los tipos de componentes registrados.
command-description-cd =
    Cambia el directorio actual de la sesión a la ruta relativa o absoluta dada.
command-description-ls-here =
    Lista el contenido del directorio actual.
command-description-ls-in =
    Lista el contenido de la ruta relativa o absoluta dada.
command-description-methods-get =
    Devuelve todos los métodos asociados con el tipo de entrada.
command-description-methods-overrides =
    Devuelve todos los métodos sobreescritos en el tipo de entrada.
command-description-methods-overridesfrom =
    Devuelve todos los métodos sobreescritos del tipo dado en el tipo de entrada.
command-description-cmd-moo =
    Hace las preguntas importantes.
command-description-cmd-descloc =
    Devuelve la cadena de localización de la descripción de un comando.
command-description-cmd-getshim =
    Devuelve el shim de ejecución de un comando.
command-description-help =
    Proporciona un resumen rápido de cómo usar toolshed.
command-description-ioc-registered =
    Devuelve todos los tipos registrados con IoCManager en el hilo actual (usualmente el hilo del juego)
command-description-ioc-get =
    Obtiene una instancia de un registro IoC.
command-description-loc-tryloc =
    Intenta obtener una cadena de localización, devolviendo null si no puede.
command-description-loc-loc =
    Obtiene una cadena de localización, devolviendo la cadena sin localizar si no puede.
command-description-physics-angular_velocity =
    Devuelve la velocidad angular de las entidades dadas.
command-description-vars =
    Proporciona una lista de todas las variables establecidas en esta sesión.
command-description-any =
    Devuelve verdadero si hay algún valor en la entrada, de lo contrario falso.
command-description-contains =
    Devuelve si el enumerable de entrada contiene el valor especificado.
command-description-ArrowCommand =
    Asigna la entrada a una variable.
command-description-isempty =
    Devuelve verdadero si la entrada está vacía, de lo contrario falso.
command-description-isnull =
    Devuelve verdadero si la entrada es nula, de lo contrario falso.
command-description-unique =
    Filtra la secuencia de entrada por unicidad, eliminando valores duplicados.
command-description-where =
    Dada alguna secuencia de entrada IEnumerable<T>, toma un bloque con firma T -> bool que decide si cada valor de entrada debe incluirse en la secuencia de salida.
command-description-do =
    Compatibilidad con BQL, aplica los comandos antiguos dados sobre la secuencia de entrada.
command-description-named =
    Filtra las entidades de entrada por su nombre, con la regex ^selector$.
command-description-prototyped =
    Filtra las entidades de entrada por su prototipo.
command-description-nearby =
    Crea una nueva lista de todas las entidades cercanas a las entradas dentro del rango dado.
command-description-first =
    Devuelve la primera entrada del enumerable dado.
command-description-splat =
    "Esparce" un bloque, valor o variable, creando N copias en una lista.
command-description-val =
    Convierte el valor, bloque o variable dado al tipo dado. Esto es principalmente un workaround para limitaciones actuales de las variables.
command-description-var =
    Devuelve el contenido de la variable dada. Intentará inferir automáticamente el tipo de la variable. Los comandos compuestos que modifican una variable pueden necesitar usar el comando 'val' en su lugar.
command-description-actor-controlled =
    Filtra entidades según si están siendo controladas activamente.
command-description-actor-session =
    Devuelve las sesiones asociadas con las entidades de entrada.
command-description-physics-parent =
    Devuelve el/los padre(s) de las entidades de entrada.
command-description-emplace =
    Ejecuta el bloque dado sobre sus entradas, con el valor de entrada colocado en la variable $value dentro del bloque.
    Adicionalmente extrae $wx, $wy, $proto, $desc, $name y $paused para entidades.
    También puede tener valores de extracción para otros tipos, consulta la documentación de ese tipo para más información.
command-description-AddCommand =
    Realiza suma numérica.
command-description-SubtractCommand =
    Realiza resta numérica.
command-description-MultiplyCommand =
    Realiza multiplicación numérica.
command-description-DivideCommand =
    Realiza división numérica.
command-description-min =
    Devuelve el mínimo de dos valores.
command-description-max =
    Devuelve el máximo de dos valores.
command-description-BitAndCommand =
    Realiza AND bit a bit.
command-description-bitor =
    Realiza OR bit a bit.
command-description-BitXorCommand =
    Realiza XOR bit a bit.
command-description-neg =
    Niega la entrada.
command-description-GreaterThanCommand =
    Realiza una comparación mayor que, x > y.
command-description-LessThanCommand =
    Realiza una comparación menor que, x < y.
command-description-GreaterThanOrEqualCommand =
    Realiza una comparación mayor o igual que, x >= y.
command-description-LessThanOrEqualCommand =
    Realiza una comparación menor o igual que, x <= y.
command-description-EqualCommand =
    Realiza una comparación de igualdad, devuelve verdadero si las entradas son iguales.
command-description-NotEqualCommand =
    Realiza una comparación de igualdad, devuelve verdadero si las entradas no son iguales.
command-description-append =
    Añade un valor al enumerable de entrada.
command-description-DefaultIfNullCommand =
    Reemplaza la entrada con el valor predeterminado del tipo si es nula, solo para tipos de valor (no objetos).
command-description-OrValueCommand =
    Si la entrada es nula, usa el valor alternativo proporcionado.
command-description-DebugPrintCommand =
    Imprime el valor dado de forma transparente, para impresiones de depuración en una ejecución de comando.
command-description-i =
    Constante entera.
command-description-f =
    Constante float.
command-description-s =
    Constante string.
command-description-b =
    Constante bool.
command-description-join =
    Une dos secuencias en una sola secuencia.
command-description-reduce =
    Dado un bloque para usar como reductor, convierte una secuencia en un solo valor.
    El lado izquierdo del bloque está implícito, y el derecho se almacena en $value.
command-description-rep =
    Repite el valor de entrada N veces para formar una secuencia.
command-description-take =
    Toma N valores de la secuencia de entrada.
command-description-spawn-at =
    Genera una entidad en las coordenadas dadas.
command-description-spawn-on =
    Genera una entidad sobre la entidad dada, en sus coordenadas.
command-description-spawn-in =
    Genera una entidad en el contenedor dado de la entidad dada, dejándola en sus coordenadas si no cabe.
command-description-spawn-attached =
    Genera una entidad adjunta a la entidad dada, en (0 0) relativo a ella.
command-description-mappos =
    Devuelve las coordenadas de una entidad relativas a su mapa actual.
command-description-pos =
    Devuelve las coordenadas de una entidad.
command-description-tp-coords =
    Teletransporta las entidades dadas a las coordenadas objetivo.
command-description-tp-to =
    Teletransporta las entidades dadas a la entidad objetivo.
command-description-tp-into =
    Teletransporta las entidades dadas "dentro" de la entidad objetivo, adjuntándola en (0 0) relativo a ella.
command-description-comp-get =
    Obtiene el componente dado de la entidad dada.
command-description-comp-add =
    Añade el componente dado a la entidad dada.
command-description-comp-ensure =
    Asegura que la entidad dada tenga el componente dado.
command-description-comp-has =
    Verifica si la entidad dada tiene el componente dado.
command-description-AddVecCommand =
    Añade un escalar (valor único) a cada elemento de la entrada.
command-description-SubVecCommand =
    Resta un escalar (valor único) de cada elemento de la entrada.
command-description-MulVecCommand =
    Multiplica un escalar (valor único) por cada elemento de la entrada.
command-description-DivVecCommand =
    Divide cada elemento de la entrada por un escalar (valor único).
command-description-rng-to =
    Devuelve un número entre la entrada (inclusivo) y el argumento (exclusivo).
command-description-rng-from =
    Devuelve un número entre el argumento (inclusivo) y la entrada (exclusivo).
command-description-rng-prob =
    Devuelve un booleano basado en la probabilidad/chance de entrada (de 0 a 1).
command-description-sum =
    Calcula la suma de la entrada.
command-description-bin =
    "Agrupa" la entrada, contando cuántas veces aparece cada elemento único.
command-description-extremes =
    Devuelve los dos extremos de una lista, entrelazados.
command-description-sortby =
    Ordena la entrada de menor a mayor por la clave calculada.
command-description-sortmapby =
    Ordena la entrada de menor a mayor por la clave calculada, reemplazando el valor con su clave calculada después.
command-description-sort =
    Ordena la entrada de menor a mayor.
command-description-sortdownby =
    Ordena la entrada de mayor a menor por la clave calculada.
command-description-sortmapdownby =
    Ordena la entrada de mayor a menor por la clave calculada, reemplazando el valor con su clave calculada después.
command-description-sortdown =
    Ordena la entrada de mayor a menor.
command-description-iota =
    Devuelve una lista de números del 1 al N.
command-description-to =
    Devuelve una lista de números del N al M.
command-description-curtick =
    El tick actual del juego.
command-description-curtime =
    El tiempo actual del juego (un TimeSpan).
command-description-realtime =
    El tiempo real desde el inicio (un TimeSpan).
command-description-servertime =
    El tiempo actual del servidor, o cero si somos el servidor (un TimeSpan).
command-description-replace =
    Reemplaza las entidades de entrada con el prototipo dado, preservando posición y rotación (pero nada más).
command-description-allcomps =
    Devuelve todos los componentes de la entidad dada.
command-description-entitysystemupdateorder-tick =
    Lista el orden de actualización por tick de los sistemas de entidades.
command-description-entitysystemupdateorder-frame =
    Lista el orden de actualización por frame de los sistemas de entidades.
command-description-more =
    Imprime el contenido de $more, es decir, cualquier extra que Toolshed no imprimió del último comando.
command-description-ModulusCommand =
    Calcula el módulo de dos valores.
    Esto es usualmente el residuo, consulta la documentación de C# para el tipo.
command-description-ModVecCommand =
    Realiza la operación de módulo sobre la entrada con el valor constante del lado derecho dado.
command-description-BitAndNotCommand =
    Realiza AND-NOT bit a bit sobre la entrada.
command-description-bitornot =
    Realiza OR-NOT bit a bit sobre la entrada.
command-description-BitXnorCommand =
    Realiza XNOR bit a bit sobre la entrada.
command-description-BitNotCommand =
    Realiza NOT bit a bit sobre la entrada.
command-description-abs =
    Calcula el valor absoluto de la entrada (eliminando el signo).
command-description-average =
    Calcula el promedio (media aritmética) de la entrada.
command-description-bibytecount =
    Devuelve el tamaño de la entrada en bytes, dado que la entrada implementa IBinaryInteger.
    Esto NO es sizeof.
command-description-shortestbitlength =
    Devuelve el número mínimo de bits necesarios para representar el valor de entrada.
command-description-countleadzeros =
    Cuenta el número de ceros binarios iniciales en el valor de entrada.
command-description-counttrailingzeros =
    Cuenta el número de ceros binarios finales en el valor de entrada.
command-description-fpi =
    pi (3.14159...) como float.
command-description-fe =
    e (2.71828...) como float.
command-description-ftau =
    tau (6.28318...) como float.
command-description-fepsilon =
    El valor epsilon para un float, exactamente 1.4e-45.
command-description-dpi =
    pi (3.14159...) como double.
command-description-de =
    e (2.71828...) como double.
command-description-dtau =
    tau (6.28318...) como double.
command-description-depsilon =
    El valor epsilon para un double, exactamente 4.9406564584124654E-324.
command-description-hpi =
    pi (3.14...) como half.
command-description-he =
    e (2.71...) como half.
command-description-htau =
    tau (6.28...) como half.
command-description-hepsilon =
    El valor epsilon para un half, exactamente 5.9604645E-08.
command-description-floor =
    Devuelve el piso del valor de entrada (redondeando hacia cero).
command-description-ceil =
    Devuelve el techo del valor de entrada (redondeando alejándose de cero).
command-description-round =
    Redondea el valor de entrada.
command-description-trunc =
    Trunca el valor de entrada.
command-description-round2frac =
    Redondea el valor de entrada al número especificado de dígitos fraccionarios.
command-description-exponentbytecount =
    Devuelve el número de bytes necesarios para almacenar el exponente.
command-description-significandbytecount =
    Devuelve el número de bytes necesarios para almacenar el significando.
command-description-significandbitcount =
    Devuelve la longitud exacta en bits del significando.
command-description-exponentshortestbitcount =
    Devuelve el número mínimo de bits para almacenar el exponente.
command-description-stepnext =
    Avanza al siguiente valor float, sumando uno al significando con acarreo.
command-description-stepprev =
    Retrocede al valor float anterior, restando uno del significando con acarreo.
command-description-checkedto =
    Convierte del tipo numérico de entrada al objetivo, generando error si no es posible.
command-description-saturateto =
    Convierte del tipo numérico de entrada al objetivo, saturando si el valor está fuera de rango.
    Por ejemplo, convertir 382 a un byte saturaría a 255 (el valor máximo de un byte).
command-description-truncto =
    Convierte del tipo numérico de entrada al objetivo, con truncamiento.
    En el caso de enteros, esto es un bit cast con extensión de signo.
command-description-iscanonical =
    Devuelve si la entrada está en forma canónica.
command-description-iscomplex =
    Devuelve si la entrada es un número complejo (por valor, no por tipo).
command-description-iseven =
    Devuelve si la entrada es par.
command-description-isodd =
    Devuelve si la entrada es impar.
command-description-isfinite =
    Devuelve si la entrada es finita.
command-description-isimaginary =
    Devuelve si la entrada es puramente imaginaria (sin parte real).
command-description-isinfinite =
    Devuelve si la entrada es infinita.
command-description-isinteger =
    Devuelve si la entrada es un entero (por valor, no por tipo).
command-description-isnan =
    Devuelve si la entrada es Not a Number (NaN).
    Este es un valor especial de punto flotante, así que esto es por valor, no por tipo.
command-description-isnegative =
    Devuelve si la entrada es negativa.
command-description-ispositive =
    Devuelve si la entrada es positiva.
command-description-isreal =
    Devuelve si la entrada es puramente real (sin parte imaginaria).
command-description-issubnormal =
    Devuelve si la entrada está en forma sub-normal.
command-description-iszero =
    Devuelve si la entrada es cero.
command-description-pow =
    Calcula la potencia de su operando izquierdo elevado al derecho. x^y.
command-description-sqrt =
    Calcula la raíz cuadrada de la entrada.
command-description-cbrt =
    Calcula la raíz cúbica de la entrada.
command-description-root =
    Calcula la raíz N-ésima de la entrada.
command-description-hypot =
    Calcula la hipotenusa de un triángulo con los lados A y B dados.
command-description-sin =
    Calcula el seno de la entrada.
command-description-sinpi =
    Calcula el seno de la entrada multiplicada por pi.
command-description-asin =
    Calcula el arcoseno de la entrada.
command-description-asinpi =
    Calcula el arcoseno de la entrada multiplicada por pi.
command-description-cos =
    Calcula el coseno de la entrada.
command-description-cospi =
    Calcula el coseno de la entrada multiplicada por pi.
command-description-acos =
    Calcula el arcocoseno de la entrada.
command-description-acospi =
    Calcula el arcocoseno de la entrada multiplicada por pi.
command-description-tan =
    Calcula la tangente de la entrada.
command-description-tanpi =
    Calcula la tangente de la entrada multiplicada por pi.
command-description-atan =
    Calcula la arcotangente de la entrada.
command-description-atanpi =
    Calcula la arcotangente de la entrada multiplicada por pi.
command-description-iterate =
    Itera la función dada sobre la entrada N veces, devolviendo una lista de resultados.
    Piensa en esto como aplicar sucesivamente la función a un valor, rastreando todos los valores intermedios.
command-description-pick =
    Elige un valor aleatorio de la entrada.
command-description-tee =
    Bifurca la entrada al bloque dado, ignorando el resultado del bloque.
    Esto esencialmente te permite tener una ramificación en tu código para hacer múltiples operaciones con un valor.
command-description-cmd-info =
    Devuelve un CommandSpec para el comando dado.
    Por sí solo, esto significa que imprimirá el mensaje de ayuda del comando.
command-description-comp-rm =
    Elimina el componente dado de la entidad.
