import React, { useState } from "react";

const RegisterPage = () => {
    const [nome, setNome] = useState("");
    const [email, setEmail] = useState("");
    const [senha, setSenha] = useState("");
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");
    const [isLoading, setIsLoading] = useState(false); // Adicionado estado de carregamento

    const handleRegister = async (e) => {
        e.preventDefault(); // Evitar recarregamento da página

        // Preparar os dados do usuário
        const userData = {
            nome,
            email,
            senhaHash: senha, // Certifique-se de que o backend espera por "senhaHash"
        };

        setIsLoading(true); // Inicia o carregamento
        setError("");
        setSuccess("");

        try {
            const response = await fetch("http://localhost:5000/api/users/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(userData),
            });

            if (response.ok) {
                const data = await response.json();
                setSuccess(data.Message || "Usuário registrado com sucesso!");
                setNome("");
                setEmail("");
                setSenha("");
            } else {
                const errorData = await response.json();
                setError(errorData.Message || "Erro ao registrar usuário.");
            }
        } catch (err) {
            setError("Erro ao conectar com o servidor. Verifique sua conexão.");
        } finally {
            setIsLoading(false); // Finaliza o carregamento
        }
    };

    return (
        <div className="container mt-5">
            <h2>Registrar</h2>
            {error && <div className="alert alert-danger">{error}</div>}
            {success && <div className="alert alert-success">{success}</div>}
            <form onSubmit={handleRegister}>
                <div className="mb-3">
                    <label htmlFor="nome" className="form-label">Nome</label>
                    <input
                        type="text"
                        id="nome"
                        className="form-control"
                        value={nome}
                        onChange={(e) => setNome(e.target.value)}
                        placeholder="Digite seu nome"
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="email" className="form-label">Email</label>
                    <input
                        type="email"
                        id="email"
                        className="form-control"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Digite seu email"
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="senha" className="form-label">Senha</label>
                    <input
                        type="password"
                        id="senha"
                        className="form-control"
                        value={senha}
                        onChange={(e) => setSenha(e.target.value)}
                        placeholder="Digite sua senha"
                        required
                    />
                </div>
                <button type="submit" className="btn btn-primary" disabled={isLoading}>
                    {isLoading ? "Registrando..." : "Registrar"}
                </button>
            </form>
        </div>
    );
};

export default RegisterPage;