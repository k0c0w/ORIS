const APIkey = 'live_y3crFyzwRT0UDfaPl7e1XN64gp6ECWICImL0Q6AdOruKYFCAFiOqLeU0vtzNhYf7';

export function GetAPIdogInfoEndpoint(dogId) {
    return `https://api.thedogapi.com/v1/breeds/${dogId}`;
} 

export function GetAPIdogImageEndpoint(reference_id) {
    return `https://api.thedogapi.com/v1/images/${reference_id}`;
}

export function GetAPIbreedsEndpoint({page, limit = 10}) {
    //return `https://api.thedogapi.com/v1/breeds?limit=${limit}&page=${page}`;
    return 'https://api.thedogapi.com/v1/breeds';
}

const APIheader = {
    'Content-Type': 'application/json',
    'x-api-key': APIkey
}

export default APIheader;