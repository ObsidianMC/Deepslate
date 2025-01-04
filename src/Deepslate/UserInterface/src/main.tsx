import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import Home from "./pages/Home";

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <MemoryRouter>
            <Routes>
                <Route path="/" element={<Home />} />
            </Routes>
        </MemoryRouter>
    </StrictMode>
);
