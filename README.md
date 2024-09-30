# Web Application to plan, attend and manage Events.
### Backend: Asp.Net 
The Backend is built with a **CQRS** (*Command and Query Responsibility Segregation*) architecture and utilize the a Mediator pattern via **MediatR**.
Additionally the backend uses a **SignalR Hub** for Real-Time Communication and **Automapper** for easier mapping between entities. 
### Frontend: Reactjs
Typescript is used in the Client Application and vite as building tool.

## To use the Application you have to add your Cloudinary Keys into API/appsettings.json
You can create an Cloudinary account here https://cloudinary.com. It's free to use to an amount of traffic.

## Starting
#### Backend
navigate into the API directory and run 
```
dotnet watch run
```
#### Frontend
navigate into the client-app directory and run
```
npm run dev
```
