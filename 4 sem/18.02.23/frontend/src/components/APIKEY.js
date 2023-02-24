const host = "https://localhost:44331/";

export const ReviewURL = `${host}review`; 

export function GetAPIdogInfoEndpoint(dogId) {
    return `${host}breeds/${dogId}`;
} 

export function GetAPIbreedsEndpoint() {
    return `${host}breeds`;
}
