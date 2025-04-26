import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const LoginPage = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false); // Estado de carregamento
    const navigate = useNavigate(); // Hook de navegação

    const handleSubmit = async (e) => {
        e.preventDefault(); // Impede o comportamento padrão do formulário

        const loginData = { email, senhaHash: password }; // Ajusta o payload para o backend

        setIsLoading(true); // Inicia o carregamento
        setError(""); // Reseta o estado de erro

        try {
            const response = await fetch("http://localhost:5000/api/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(loginData),
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData?.Message || "Erro de autenticação.");
            }

            const data = await response.json();
            if (data.Token) { // Certifique-se de usar o nome correto retornado pelo backend
                localStorage.setItem("token", data.Token); // Salva o token no localStorage
                alert("Login bem-sucedido!");
                navigate("/empresa-search"); // Redireciona para a página EmpresaSearch
            }
        } catch (error) {
            console.error("Erro durante o login:", error.message);
            setError(error.message); // Define a mensagem de erro
        } finally {
            setIsLoading(false); // Finaliza o carregamento
        }
    };

    return (
        <div className="container mt-5">
            <h2>Login</h2>
            {error && <div className="alert alert-danger">{error}</div>} {/* Mostra erro, se houver */}
            <form onSubmit={handleSubmit}>
                <div className="mb-3">
                    <label htmlFor="email" className="form-label">Email</label>
                    <input
                        type="email"
                        id="email"
                        className="form-control"
                        placeholder="Digite seu email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="password" className="form-label">Senha</label>
                    <input
                        type="password"
                        id="password"
                        className="form-control"
                        placeholder="Digite sua senha"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit" className="btn btn-primary" disabled={isLoading}>
                    {isLoading ? "Entrando..." : "Entrar"}
                </button>
            </form>
        </div>
    );
};

export default LoginPage;