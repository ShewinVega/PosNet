
# PostNet

This project shows one of the easies way to handle JWT tokens with clean arquitecture. Furthemore the project is not closed to changes yet, I will be adding new modules and some specific things like pdf generator, middlewares,  handle exceptions errors, etc...



## Environment Variables

To run this project, you will need to add the following environment variables to your user-secrets.

`Jwt:secretKey`

`Jwt:Issuer`

`Jwt:Audience`

For initializing the user-secrets and add these variables you have to run these commands sequently:

```bash
  dotnet user-secrets init
  dotnet user-secrets set "Variable Name" "Variable Content"
```
    
## License

[MIT](https://choosealicense.com/licenses/mit/)

