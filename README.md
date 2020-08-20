# Overview
<img src="https://github.com/bitasuperactive/KCIBasic/blob/master/doc/icon.ico" width="192"/> <img src="https://github.com/bitasuperactive/KCIBasic/blob/master/doc/kcibasicimage.png" width="436"/>

Kaspersky Custom Installer ("KCI" en adelante) es una utilidad para los amantes de la ciberseguridad rusa que facilita acceder a todas las funcionalidades de los principales productos domésticos de Kaspersky Lab sin perder un solo euro. Inspirado en mi pobreza, esta herramienta proporciona la forma más sencilla y segura de llevar a cabo esta azaña.

[Utilidades](https://github.com/bitasuperactive/KCIBasic/blob/master/README.md#principales-utilidades) | [Descargas y Notas de lanzamiento](https://github.com/bitasuperactive/KCIBasic/releases) | [Ejecución](https://github.com/bitasuperactive/KCIBasic/blob/master/README.md#ejecución) | [Problemas sin resolver](https://github.com/bitasuperactive/KCIBasic/blob/master/README.md#problemas-sin-resolver)


## Estado
:stop_sign: Versión no estable.


## Productos domésticos soportados
* :turtle: Kaspersky Antivirus.
* :dragon_face: Kaspersky Internet Security.
* :dragon: Kasperksy Total Security.


## Principales utilidades
<img src="https://github.com/bitasuperactive/KCIBasic/blob/master/doc/uninstallimage.png" width="150"/> <img src="https://github.com/bitasuperactive/KCIBasic/blob/master/doc/cleanimage.jpg" width="150"/> <img src="https://github.com/bitasuperactive/KCIBasic/blob/master/doc/downloadimage.png" width="150"/> <img src="https://github.com/bitasuperactive/KCIBasic/blob/master/doc/keyimage.jpg" width="150"/>

### Desinstalación de Kaspersky
Ya sea de forma manual (*Instalación habitual*) o automática (*Instalación rápida*) se facilita la desintalación del producto Kaspersky Lab instalado en el sistema operativo del usuario, función imprescindible para modificar los Registros de Windows referentes al Antivirus en cuestión sin perjudicar la seguridad del equipo.

:information_source: En caso de disponer de otros productos Anti-Malware o en su defecto, Kaspersky Custom Installer saltará este paso y el propio asistente de instalación de Kaspersky Lab informará al usuario de cualquier incompatibilidad o pasos adicionales necesarios.

### Limpieza del Registro de Windows
De forma automática, purga de las siguientes claves del Registro de Windows permitiéndo renovar licencias de evaluación caducadas:
> HKEY_LOCAL_MACHINE\SOFTWARE\KasperskyLab            
> HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\RNG
> HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates

:information_source: Estas claves de registro se encuentran directamente relacionadas con los productos de Kaspersky, modificarlas no afectará de forma alguna al sistema operativo.

### Asistente de instalación "Online" o "Full-Package"
Mediante el check box *Offline setup* se descargará automáticamente, desde de los servidores oficiales de Kaspersky, la versión completa del asistente de instalación, de gran utilidad si la versión habitual les presenta problemas.

:warning: Se requiere la versión completa del asistente de instalación (*Offline Setup*) para poder realizar una *Instalación Rápida*.

### Licencias de evaluación funcionales
Actualización mensual de las licencias de evaluación para alargar esos 31 días siempre que sea posible.


## Otras utilidades
### Desinstalación automática de Kaspersky Secure connection
Desinstalación automática del producto *Kaspersky Secure connection*.

:information_source: El usuario puede habilitar el check box *Kaspersky Secure connection* para mantener este producto.

### Activar producto
Sin necesidad de volver a realizar una instalación habitual o rápida, se intentará activar el producto Kaspersky Lab instalado con las licencias de evaluación activas en el momento.

:warning: Es necesario actualizar Kaspersky Custom Installer para obtener las últimas licencias disponibles.

### Extraer configuración
Para no perder los ajustes modificados dentro de las aplicaciones Kaspersky Lab, esta utilidad permite extraer un archivo *.cfg* el cual podremos reimportar una vez reinstalemos el producto evitando molestias.


## Ejecución
### Requisitos
:one: Permisos de Administrador.  
:two: Conexión a internet.

### Problemas sin resolver
* Detectado como virus troyano *Gen.Variant.MSILPerseus* (falso positivo).
* Los enlaces de descarga directos para los asistentes de instalación completos (*Offline Setups*) no son estables.
* La aplicación no se actualiza automáticamente ni avisa al usuario de haber una actualizanción disponible.
* Código spaguetti.

### Falsos positivos
![image](https://github.com/bitasuperactive/KCIBasic/blob/master/doc/virustotalimage.png)

[Virus total](https://www.virustotal.com/gui/file/24f97e787c5fbb600f6643bcb957f68ab099f12a7e37fc6473feb582d19c40e3/detection) deteca a Kaspersky Custom Installer como un troyano *Gen.Variant.MSILPerseus* debido a las funciones automáticas que lleva a cabo sin informar al usuario, como, por ejemplo, la descarga y ejecución automática del asistente de instalación de Kaspersky Lab. Esto es un *falso positivo*. Revisando el código fuente podréis comprobar que esta aplicación no lleva a cabo ninguna función maliciosa ni mucho menos propia de un troyano.

:warning: Por el momento, es posible que sea necesario añadir Kaspersky Custom Installer a "exclusiones" en Windows Defender u otros aplicativos Anti-Malware.
