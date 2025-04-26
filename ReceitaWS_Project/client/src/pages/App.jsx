import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./LoginPage";
import EmpresaSearch from "./EmpresaSearch";

const App = () => {
    return (
        <Router>
            <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route path="/empresa-search" element={<EmpresaSearch />} />
            </Routes>
        </Router>
    );
};

export default App;