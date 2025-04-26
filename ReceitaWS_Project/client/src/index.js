import React, { useState } from "react";
import ReactDOM from "react-dom";
import "bootstrap/dist/css/bootstrap.min.css";

const App = () => {
    const [showLogin, setShowLogin] = useState(false); // Estado para alternar entre login e registro

    const handleLoginClick = () => {
        setShowLogin(true);
    };

    const handleRegisterClick = () => {
        setShowLogin(false);
    };

    return (
        <div className="container mt-5 text-center">
            <h1>ReceitaWS Project</h1> {/* Título da aplicação */}
            <div className="mt-4">
                {/* Botões para alternar entre Login e Registrar */}
                <button
                    className="btn btn-primary mx-2"
                    onClick={handleLoginClick}
                >
                    Login
                </button>
                <button
                    className="btn btn-secondary mx-2"
                    onClick={handleRegisterClick}
                >
                    Registrar
                </button>
            </div>
            <div className="mt-5">
                {/* Renderiza Login ou Registrar com base no estado */}
                {showLogin ? (
                    <LoginComponent />
                ) : (
                    <RegisterComponent />
                )}
            </div>
        </div>
    );
};

// Componente de Login
const LoginComponent = () => (
    <div>
        <h2>Login</h2>
        <form>
            <div className="mb-3">
                <label htmlFor="email" className="form-label">Email</label>
                <input type="email" className="form-control" id="email" />
            </div>
            <div className="mb-3">
                <label htmlFor="password" className="form-label">Senha</label>
                <input type="password" className="form-control" id="password" />
            </div>
            <button type="submit" className="btn btn-primary">Entrar</button>
        </form>
    </div>
);

// Componente de Registro
const RegisterComponent = () => (
    <div>
        <h2>Registrar</h2>
        <form>
            <div className="mb-3">
                <label htmlFor="name" className="form-label">Nome</label>
                <input type="text" className="form-control" id="name" />
            </div>
            <div className="mb-3">
                <label htmlFor="email" className="form-label">Email</label>
                <input type="email" className="form-control" id="email" />
            </div>
            <div className="mb-3">
                <label htmlFor="password" className="form-label">Senha</label>
                <input type="password" className="form-control" id="password" />
            </div>
            <button type="submit" className="btn btn-secondary">Registrar</button>
        </form>
    </div>
);

ReactDOM.render(<App />, document.getElementById("root"));