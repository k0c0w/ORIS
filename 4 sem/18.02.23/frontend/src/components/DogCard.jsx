import React, { useEffect, useState } from 'react'
import { NavLink, useParams } from 'react-router-dom';
import {Button, Card} from 'antd'

import { GetAPIdogInfoEndpoint } from './APIKEY';

const info = {"weight":{"imperial":"6 - 13","metric":"3 - 6"},"height":{"imperial":"9 - 11.5","metric":"23 - 29"},"id":1,"name":"Affenpinscher","bred_for":"Small rodent hunting, lapdog","breed_group":"Toy","life_span":"10 - 12 years","temperament":"Stubborn, Curious, Playful, Adventurous, Active, Fun-loving","origin":"Germany, France","reference_image_id":"BJa4kxc4X","image":{"id":"BJa4kxc4X","width":1600,"height":1199,"url":"https://cdn2.thedogapi.com/images/BJa4kxc4X.jpg"}}

const {Meta} = Card;

const noInfo = '-';

const style = {
  display:'flex', justifyContent:'center', width:'100%', alignItems:'center', height:'100%', flexDirection:'column'
}

const DogCard = () => {
  const {dogId} = useParams();
  const [error, setError] = useState(null);
  const [infoIsLoaded, setInfoIsLoaded] = useState(false);
  const [dog, setDog] = useState({});
  useEffect(() =>
    {
      fetch(GetAPIdogInfoEndpoint(dogId))
        .then(result => result.json())
        .then(
          (result) => { 
            setInfoIsLoaded(true);
            setDog(result);
          },
          (error) => {
            setInfoIsLoaded(true);
            setError(error);
          }
        )
    }, []
  );

  if(error){
    return (<div>Loading error: {error.message}</div>);
  } else if(!infoIsLoaded){
    return (<div>Loading card...</div>);
  }

  return (
    <div style={style}>
      { !dog.error &&
        <Card size={'default'} hoverable
        cover={<img alt="dog image" src={dog.image_url} style={{width:400, display:'flex', alignItems:'center'}}/>}
        >
          <Meta title={dog.name} description={dog.breed_group && `${dog.breed_group} dog`}/>
          <div>
              <h4>Temperament</h4>
              <div><p>{dog.temperament ? dog.temperament : noInfo}</p></div>
              { dog.origin && <>
                <h4>Origin</h4>
                <div><p>{info.origin}</p></div>
              </>
              }
              {(dog.breed_for && !(dog.breed_for === '')) && <><h4>Description</h4>
                <div><p>{dog.breed_for}</p></div>
                </>
              }
              {(dog.min_weight && dog.max_weight) && <div><p>weight: {dog.min_weight} - {dog.max_weight}</p></div>}
              {dog.min_height && dog.max_height && <div><p>height: {dog.min_height} - {dog.max_height}</p></div> }
              {dog.life_span && <div><p>lifespan: {dog.life_span}</p></div>}
          </div>
          <div style={{display:"flex", justifyContent:"center"}}>
            <NavLink to={`/#${dogId}`}><Button>Back to breeds</Button></NavLink>
          </div>
      </Card>}
      {dog.error && <div>{dog.error}</div>}
      </div>
  );
}

export default DogCard