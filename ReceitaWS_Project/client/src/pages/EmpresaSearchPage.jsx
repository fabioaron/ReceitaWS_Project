import React, { useState } from "react";

const EmpresaSearch = () => {
    const [cnpj, setCnpj] = useState("");
    const [empresa, setEmpresa] = useState(null);
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    const handleSearch = async () => {
        if (!cnpj || cnpj.length !== 14 || isNaN(cnpj)) {
            setError("CNPJ inválido. Certifique-se de inserir 14 dígitos numéricos.");
            setEmpresa(null);
            return;
        }

        setIsLoading(true);
        setError("");
        setEmpresa(null);

        try {
            const response = await fetch(`http://localhost:5000/api/empresas/${cnpj}`, {
                method: "GET",
            });

            if (response.ok) {
                const data = await response.json();
                setEmpresa(data);
                setError("");
            } else if (response.status === 404) {
                // Empresa não encontrada no banco, tenta registrar
                const registerResponse = await fetch("http://localhost:5000/api/empresas/register", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ cnpj }),
                });

                if (registerResponse.ok) {
                    const registerData = await registerResponse.json();
                    setEmpresa(registerData.Empresa || registerData);
                    setError("");
                } else {
                    const registerError = await registerResponse.json();
                    setError(registerError.Message || "Erro ao registrar empresa.");
                }
            } else {
                const errorData = await response.json();
                setError(errorData.Message || "Erro ao buscar empresa.");
            }
        } catch (err) {
            setError("Erro ao conectar com o servidor.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="container mt-5">
            <h2>Buscar Empresa</h2>
            {error && <div className="alert alert-danger">{error}</div>}
            {empresa && (
                <div className="alert alert-success">
                    <h4>Dados da Empresa:</h4>
                    <p><strong>Nome:</strong> {empresa.nome}</p>
                    <p><strong>CNPJ:</strong> {empresa.cnpj}</p>
                    <p><strong>Endereço:</strong> {empresa.endereco}</p>
                    <p><strong>Data de Registro:</strong> {new Date(empresa.dataRegistro).toLocaleDateString()}</p>
                </div>
            )}
            <div className="mb-3">
                <label htmlFor="cnpj" className="form-label">CNPJ</label>
                <input
                    type="text"
                    id="cnpj"
                    className="form-control"
                    placeholder="Digite o CNPJ (apenas números)"
                    value={cnpj}
                    onChange={(e) => setCnpj(e.target.value)}
                    maxLength={14}
                    required
                />
            </div>
            <button
                onClick={handleSearch}
                className="btn btn-primary"
                disabled={isLoading}
            >
                {isLoading ? "Buscando..." : "Buscar Empresa"}
            </button>
        </div>
    );
};

export default EmpresaSearchPage;