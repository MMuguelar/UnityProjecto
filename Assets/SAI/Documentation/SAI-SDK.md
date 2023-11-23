
SDK Unity for SpaceAI intetegrations &middot; 
============================================================================

[![](https://miro.medium.com/v2/resize:fit:3708/1*P78OYMZUQU2KnkFogM8DjA.jpeg)](https://www.spaceai.com)
Indice

- [[Api]]
    
- [[Clusters]]
    
- [[Devices]]
    
- [[LocationManager]]
    
- [[Login]]
    
- [[Nodos]]
    
- [[SDK/Util|Util]]
    
- [[Wallet]]
-------------------------------------------------------------------------------

Brief explanation of how the API was designed and the constraints it presents.

The API was designed by integrating the existing systems, adapting them for improved accessibility and scalability. The API requires a prefab object available in the Unity scene. This prefab can be located under the folder SAI/Prefabs inside the package


----------------------------------------------------------------------------

<<Explicación de que librerias depende su funcionamiento.
Links de referecias a las mismas.>>

The following libraries are used at some point by the SDK and are already integrated into the SDK package.

Bitsplash - (Calendary) 
Fisheye - (3D World Map Rendering)
Demigiant - ( DOTWeen - UI Animations )
NativeFilePicker - ( File Explorer )
NativeGallery - ( Image & File Previews )
Phonenumberlib - ( Phone Formatter )
Pixelplacement - ( UI Addon )
SimpleFileBrowser - ( File Explorer for Android )
StandaloneFileBrower - ( File Browser )
IPFS - ( File Upload/Download)
JsonDotNet - ( Serializer/Converter )
JSONReader - ( Serializer/Converter )

las siguientes librerias  NO estan incluidas y deben ser instaladas manualmentedes de el Package Manager.

TextMeshPro - ( UI Texts & Fonts ) 
Mathematics - ( Complex Algorithm Lib ) 
NewtonSoft Json - Instalar manualmente desde el Administrador de paquetes mediante la url :  com.unity.nuget.newtonsoft-json

![[Pasted image 20231121101935.png]]



----------------------------------------------------------------------------

<a name="top"></a>
# Contents
- ###  [API Documentation](index.md)
- ### [Installing the Plugin](SAI-SDK.md#^d32198)
- ### [Setup Guides]
- ### [Configure your License]
- ### [Using the plugin]
- ### [Privacy Policy](help/PRIVACY_POLICY.md)


## Setup Guides


STEP 1 - Descargar la SDK del siguiente link=https://drive.google.com/file/d/1HRh4nhoe-j3JFREiLi_IyjvPNpknoV9m/view?usp=sharing 

![[Pasted image 20231117120255.png]]



STEP 2 - Importar el paquete a Unity


STEP 3 - Instalar dependencias desde Package Manager :
TextMeshPro 
Mathematics 
Newtonsoft Json

( Ver guia de instalacion arriba )


STEP 4 - Reiniciar Unity

STEP 5 - Dentro del proyecto ubicar el prefab SAI dentro de la carpeta SAI/Prefabs y colocarlo en la escena


## Configure your license (To be define)

1. Login to Customer Dashboard to generate an application key:

2. Add your license-key to 
   `src/main/Manifest.xml`:

```diff
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
  <application
    android:name=".MainApplication"
    android:allowBackup="true"
    android:label="@string/app_name"
    android:icon="@mipmap/ic_launcher"
    android:theme="@style/AppTheme">

    <!-- SAI licence -->
+     <meta-data android:name="com.spai.license" android:value="YOUR_LICENCE_KEY_HERE" />
    .
    .
    .
  </application>
</manifest>
```

## Usando la SDK##

Usar la SDK es muy simple, tipeando SAI.SDK. accederemos a la misma desde cualquier parte de nuestro codigo. 
Ejemplo :
para enviar un mensaje al usuario 
SAI.SDK.Util.errorHandler.ShowPopup("Esto es un mensaje","Titulo");



[Subir](#top)
## 

# License

The MIT License (MIT)

Copyright (c) 2018 Chris Scott, Transistor Software

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.