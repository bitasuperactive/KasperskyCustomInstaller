# Overview
<img src="https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/doc/icon.ico" width="192"/> <img src="https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/doc/GUI_showcase.png" width="436"/>

**Kaspersky Custom Installer** (**KCI** en adelante) es una alternativa de código abierto al software conocido como Kaspersky Reset Trial o KRT.

KCI soporta los siguientes productos domésticos de Kaspersky Lab: Kaspersky Antivirus / Kaspersky Internet Security / Kaspersky Total Security.

[Utilidades](https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/README.md#principales-utilidades) | [Descargas y Notas de lanzamiento](https://github.com/bitasuperactive/KasperskyCustomInstaller/releases) | [Ejecución](https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/README.md#ejecución) | [Problemas sin resolver](https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/README.md#problemas-sin-resolver)

## Funciones principales
<img src="https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/doc/uninstallimage.png" width="150"/> <img src="https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/doc/cleanimage.jpg" width="150"/> <img src="https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/doc/downloadimage.png" width="150"/> <img src="https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/doc/keyimage.jpg" width="150"/>

- **Desinstalación de Kaspersky manual/automática:**

  Ya sea de forma manual (*Instalación habitual*) o automática (*Instalación rápida*), facilita la desintalación del producto correspondiente, en caso de estar instalado en el sistema operativo del usuario. Función imprescindible para modificar los registros de Windows referentes al producto.

  :information_source: En caso de disponer de otros productos Anti-Malware, el propio asistente de instalación del producto informará al usuario de cualquier incompatibilidad y acciones adicionales requeridas.

- **Limpieza del registro de Windows:**

  Purga las siguientes claves de registro permitiéndo renovar licencias de evaluación caducadas:
  > HKEY_LOCAL_MACHINE\SOFTWARE\KasperskyLab            
  > HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\RNG
  > HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates

  :warning: Permisos de administrador requeridos.

  :information_source: Estas claves de registro son originarias de los productos de Kaspersky y modificarlas no afectará de forma alguna al sistema operativo del usuario.

- **Asistente de instalación Online/Full-Package:**

  Al ticar la opción *Offline setup*, KCI descarga, desde de los servidores oficiales del producto, la versión completa del asistente de instalación. Función de gran utilidad si la versión *online* presenta problemas de descarga.

  :information_source: Al realizar una *Instalación Rápida*, KCI descargará la versión completa del asistente de instalación del producto para llevar a cabo una instalación silenciosa.

- **<s>Licencias de evaluación ampliada:</s>**

  <s>Facilita, si las hubiera, licencias de evaluación ampliadas.</s>

## Funciones adicionales
- **Desinstalación automática de productos no deseados:**

  Desinstala automáticamente el producto *Kaspersky Secure connection*, software adicional habitualmente no deseado.

  :information_source: El usuario puede ticar la opción *Kaspersky Secure connection* para mantener este producto.

- **<s>Activación directa del producto:</s>**

  <s>Mediante el botón *Activar Producto*, sin necesidad de volver a realizar una instalación habitual o rápida, intentará activar el producto instalado con las licencias de evaluación activas en el momento.</s>

  :warning: <s>Es necesario actualizar KCI para obtener las últimas licencias disponibles.</s>

- **Exportación de la configuración de usuario:**

  Realiza una copia de seguridad de la configuración de usuario del producto, en formato *.cfg*.


## Problemas sin resolver
1. Falso positivo: *Gen.Variant.MSILPerseus*
  
2. Los enlaces de descarga directos para los asistentes de instalación completos (*Offline Setups*) no son estables.
  
3. La aplicación no se actualiza automáticamente ni avisa al usuario de haber una actualizanción disponible.
 
4. Código spaguetti.


## Falsos positivos
<img src="https://github.com/bitasuperactive/KasperskyCustomInstaller/blob/modernUI/doc/virustotalimage.png" width="300"/>

Las detecciones proporcionadas por [Virus total](https://www.virustotal.com/gui/file/24f97e787c5fbb600f6643bcb957f68ab099f12a7e37fc6473feb582d19c40e3/detection) se deben a las funciones automáticas que KCI realiza sin intervención del usuario, como la descarga y ejecución automática del asistente de instalación de Kaspersky.

:warning: Es posible que sea necesario añadir KCI al listado de exclusiones de Windows Defender y/u otros programas Anti-Malware de su sistema.

## Disclaimer  
**Al usar esta aplicación asumes toda responsabilidad subyacente de la violación del EULA aceptada al instalar los productos Kaspersky Lab, entre otras normativas.**   
*KCI es simplemente mi primer proyecto personal experimentándo con el lenguaje C#.*
