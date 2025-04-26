import React from "react";
import ReactDOM from "react-dom";
import App from "./App"; // Importa o componente principal do app
import { BrowserRouter as Router } from "react-router-dom"; // Configura o roteador
import "bootstrap/dist/css/bootstrap.min.css"; // Opcional, se estiver usando Bootstrap

ReactDOM.render(
    <React.StrictMode>
        <Router>
            <App />
        </Router>
    </React.StrictMode>,
    document.getElementById("root"