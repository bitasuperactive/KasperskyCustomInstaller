# Overview
Kaspersky Custom Installer es una utilidad para los amantes de Kaspersky que facilita acceder a todas las funcionalidades de los principales productos domésticos de Kaspersky Lab sin perder un solo euro. Inspirado en mi pobreza, esta herramienta proporciona la forma más sencilla y segura de llevar a cabo esta azaña.

[Descargas y Notas de lanzamiento](https://github.com/bitasuperactive/KCIBasic/releases)


![kavimage](https://github.com/bitasuperactive/KCIBasic/blob/master/doc/kavimage.jpg)


## Build status
:stop_sign: Versión no estable.


## Supported Kaspersky Lab Editions
1. :small_red_triangle_down: Kaspersky Antivirus.
1. :small_orange_diamond: Kaspersky Internet Security (recomendado).
1. :large_orange_diamond: Kasperksy Total Security.


## Main KCI Utilities
### Desinstalación de Kaspersky
Ya sea de forma manual (*Instalación habitual*) o automática (*Instalación rápida*) se facilita la desintalación del producto Kaspersky Lab instalado en el sistema operativo del usuario, paso imprescindible para poder modificar los Registros de Windows referentes al Antivirus en cuestión sin perjudicar la seguridad de nuestro equipo.

### Limpieza del Registro de Windows
De forma automática, purga de las siguientes claves del Registro de Windows permitiéndo renovar licencias de evaluación caducadas:
*HKEY_LOCAL_MACHINE\SOFTWARE\KasperskyLab*              
*HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\RNG*
*HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates*

:warning: Este procedimiento altera únicamente registros referentes a la aplicación en cuestión. Registros que no deberían estar ahí pero Kaspersky los deja para evitar renovaciones fraudulentas de licencias.

### Asistente de instalación "Online" o "Full-Package"
Mediante el check *Offline setup* descarga automática (directamente de los servidores oficiales de Kaspersky) de la versión completa del asistente de instalación, para aquellos que la versión "común" u "online" no les funciona como debería (velocidad de descarga reducida).

:warning: Es necesario descargar la versión completa del asistente de instalación, es decir marcar el check *Offline setup*, para poder realizar una *Instalación rápida*.

### Licencias de evaluación funcionales
Actualización mensual de las últimas licencias de evaluación funcionales, para alargar esos 31 días siempre que sea posible.


## Other KCI Utilities
### Desinstalación automática de Kaspersky Secure connection
Si no habilitas el check correspondiente, este producto se desinstalará automáticamente.

### Activar Producto
Sin necesidad de volver a realizar una instalación habitual o rápida, se intentará activar el producto con las licencias de evaluación activas en el momento.

:warning: Importante actualizar Kaspersky Custom Installer para obtener las últimas licencias.

### Extraer Configuración
Para no perder los ajustes modificados dentro de la aplicación, esta utilidad permite extraer un archivo *.cfg* el cual podremos reimportar una vez reinstalemos el producto, de esta forma evitamos molestias.


## Running KCI
Es imprescindible ejecutar la aplicación como administrador para poder llevar a cabo las funciones anteriormente mencionadas.

### Problemas sin resolver
:one: Las URLs para los asistentes de instalación completos a veces fallan.

:two: La aplicación no se actualiza automáticamente ni avisa al usuario de haber una actualizanción disponible.

:three: Código spaguetti.


## Falsos positivos
![virustotalimage](https://github.com/bitasuperactive/KCIBasic/blob/master/doc/virustotalimage.png)

[Virus total](www.virustotal.com/gui/file/24f97e787c5fbb600f6643bcb957f68ab099f12a7e37fc6473feb582d19c40e3/detection) deteca Kaspersky Custom Installer como un troyano "Gen.Variant.MSILPerseus" debido a las funciones automáticas que lleva a cabo el mismo sin informar al usuario, como, por ejemplo, la descarga y ejecución automática del asistente de instalación de Kaspersky Lab (mencionados en el apartado KCI Utilities). Esto es un "falso positivo", mediante el código fuente de la aplicación se puede comprobar el comportamiento de la misma y confirmar esta afirmación.
