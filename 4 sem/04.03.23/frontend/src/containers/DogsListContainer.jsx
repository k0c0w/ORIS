<<<<<<< HEAD
import React, { useEffect, useState } from "react";
import DogsList from "../components/DogsList";
import { GetAPIbreedsEndpoint } from "../components/APIKEY";
import NotFoundPage from "./NotFoundPage";
import FilterFolder from "./FilterFolder";
import { Content } from "antd/es/layout/layout";

const contentStyle = {
    color: 'white',
    backgroundColor: '#108ee9',
  };



export default function DogsListContainer({props}) {
    const [dogs, setDogs] = useState([]);
    const [totalDogs, setTotalDogs] = useState(0);
    const [page, setPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(5);
    const [isError, setIsError] = useState(false);
    const [filterQuery, setFilterQuery] = useState("");

    function changeBreedsArray(page, itemsPerPage) {
        fetch(`${GetAPIbreedsEndpoint(page, itemsPerPage)}&${filterQuery}`)
        .then(respone => respone.json())
        .then(
            (result) => {
                setDogs(result.breeds);
                setTotalDogs(result.totalInStorage);
                setIsError(false);
            },
            (error) => {
                console.log(error);
                setIsError(true);
            }
        )
    }

    useEffect(() => { changeBreedsArray(page, itemsPerPage) }, [filterQuery, page, itemsPerPage]); 

    if(isError)
        return <NotFoundPage>404</NotFoundPage>;

    return (
        <Content style={contentStyle}>
            <FilterFolder setFilterQuery={setFilterQuery} page={page} itemsPerPage={itemsPerPage}/>
            <DogsList {...props} dogs = {dogs} totalDogs = {totalDogs} 
            itemsPerPage={itemsPerPage} page = {page}
            setPage = {(page) =>{setPage(parseInt(page));}} 
            setItemsPerPage = {(items) => {setItemsPerPage(parseInt(items)); changeBreedsArray(page, items);}}/>
        </Content>
        );
}
=======
import React from "react";
import DogsList from "../components/DogsList";
import { GetAPIbreedsEndpoint, TotalURL } from "../components/APIKEY";

class DogsListContainer extends React.Component {
    constructor(props){
        super(props);
        this.state = { 
            dogs: [],
            total: 0
        }
    }

    componentDidMount() {
        fetch(TotalURL)
        .then((response) => response.json())
        .then(result => {this.setState({total: result.total, dogs: this.state.dogs});
                        return fetch(GetAPIbreedsEndpoint(1,300));})
        .then(result => result.json())
        .then(result => this.setState({total: this.state.total, dogs:result}))
        .catch(err => console.log("fetch error:" + err))
    }

    render() {
        return (
            <DogsList {...this.props} dogs = {this.state.dogs} total = {this.state.total}/>
        );
    }
}

export default DogsListContainer;
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
