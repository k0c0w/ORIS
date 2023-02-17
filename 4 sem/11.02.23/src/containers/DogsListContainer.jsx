import React from "react";
import DogsList from "../components/DogsList";
import { GetAPIbreedsEndpoint } from "../components/APIKEY";

class DogsListContainer extends React.Component {
    constructor(props){
        super(props);
        this.state = { 
            page: 0,
            dogs: []
        }
    }

    componentDidMount() {
        fetch(GetAPIbreedsEndpoint(this.state.page, 200))
        .then(result => result.json())
        .then(result => this.setState({page:1, dogs:result}))
    }

    render() {
        return (
            <DogsList {...this.props} dogs = {this.state.dogs}/>
        );
    }
}

let mapStateToProps = (state) => ({}) 

export default DogsListContainer;