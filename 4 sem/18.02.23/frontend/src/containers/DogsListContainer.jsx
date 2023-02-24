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