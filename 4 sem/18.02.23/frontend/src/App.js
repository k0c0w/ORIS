import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import React from "react";
import DogCard from './components/DogCard';
import NotFoundPage from './containers/NotFoundPage';
import DogsListContainer from './containers/DogsListContainer';

function App() {
  return (
      <Router>
        <Routes>
          <Route path="/" element={<DogsListContainer/>}/>
          <Route path="/breeds/:dogId" element={<DogCard/>}/>
          <Route path='*' exact={true} element={<NotFoundPage/>}/>
        </Routes>
      </Router>
    );
}

export default App;