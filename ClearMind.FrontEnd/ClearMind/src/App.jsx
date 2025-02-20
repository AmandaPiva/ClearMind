import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import LandinPage from "./Pages/LandinPage/LandinPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LandinPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
