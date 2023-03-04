import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
<<<<<<< HEAD
import React, { useState } from "react";
=======
import React from "react";
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
import DogCard from './components/DogCard';
import NotFoundPage from './containers/NotFoundPage';
import DogsListContainer from './containers/DogsListContainer';
import ReviewForm from './components/ReviewForm';

function App() {
<<<<<<< HEAD

  return (
      <Router>
        <Routes>
          <Route path="/" element={<DogsListContainer/>}/>
=======
  return (
      <Router>
        <Routes>
          <Route path="/"  element={<DogsListContainer/>}/>
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
          <Route path="/breeds/:dogId" exact={true} element={<DogCard/>}/>
          <Route path="/review" exact={true} element={<ReviewForm/>}/>
          <Route path='*' element={<NotFoundPage/>}/>
        </Routes>
      </Router>
    );
}

export default App;