## Login 🔐  SAI.SDK.Login


El sistema de inicio de sesión sirve tanto para iniciar sesión como para el registro de nuevos usuarios.

Los Metodos de esta Clase utilizan eventos a los que debemos estar subscriptos.
Recuerden que la subscripcion se hace en el metodo OnEnable y se debe desubscribir en el metodo OnDisable.

  

#### Métodos:

- SignIn 🚪: Iniciar sesión.

![[Pasted image 20231121095835.png]]

PASO 1 - Log In :

    void Start()
    {
	SAI.SDK.Login.SignIn("user","password");
	}

PASO 2 - Esperar respuesta

    private void OnEnable()
    {
        LoginSystem.LoginSuccessTrigger += MySuccessFunction;
        LoginSystem.LoginFailureTrigger += MyFailureFunction;
    }
    private void OnDisable()
    {
        LoginSystem.LoginSuccessTrigger -= MySuccessFunction;
        LoginSystem.LoginFailureTrigger -= MyFailureFunction;
    }
    
    private void MySuccessFunction()
    {
    print("Login Successfull"); 
    //En este punto la SessionKey del usuario se almacena en SAI.SDK.Login.SessionKey;
    }
    
- SignUp 📝: Registrar un nuevo usuario.

PASO 1 - Registrar

void Start()
{
SAI.SDK.Login.SignUp("email","username","telefono");
}

PASO 2 - Esperar Respuesta 

private void OnEnable()
    {
        LoginSystem.RegistrationSuccessTrigger += MySuccessFunction;
        LoginSystem.RegistrationFailureTrigger += MyFailureFunction;
    }
    
- SignOut 🚪: Cerrar sesión.
 SAI.SDK.Login.Logout();
    

    
  

### Propiedades: 

- SessionKey 🔑: Identificador único de sesión que proporciona acceso a otros métodos. La variable se inicializa al momento de un inicio de session exitoso.
    

  
### Eventos:

LoginSuccessTrigger
LoginFailureTrigger
LogoutSuccessTrigger
RegistrationSuccessTrigger
RegistrationFailureTrigger

Los eventos se registrar en el metodo OnEnable y se eliminan en el metodo OnDisable

```
private void OnEnable()
{
    LoginSystem.LoginSuccessTrigger += MiMetodo;
    LoginSystem.LoginFailureTrigger += MiOtroMetodo;
}
private void OnDisable()
{
    LoginSystem.LoginSuccessTrigger -= MiMetodo;
    LoginSystem.LoginFailureTrigger -= MiOtroMetodo;
}

private void MiMetodo()
{
print("Metodo Invocado"); 
}
```

