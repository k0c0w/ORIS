import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import React from "react";
import DogCard from './components/DogCard';
import NotFoundPage from './containers/NotFoundPage';
import DogsListContainer from './containers/DogsListContainer';
import ReviewForm from './components/ReviewForm';

function App() {
  return (
      <Router>
        <Routes>
          <Route path="/"  element={<DogsListContainer/>}/>
          <Route path="/breeds/:dogId" exact={true} element={<DogCard/>}/>
          <Route path="/review" exact={true} element={<ReviewForm/>}/>
          <Route path='*' element={<NotFoundPage/>}/>
        </Routes>
      </Router>
    );
}

export default App;