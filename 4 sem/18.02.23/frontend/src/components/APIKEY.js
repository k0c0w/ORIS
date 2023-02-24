const host = "https://localhost:44331/";

export const ReviewURL = `${host}review`; 

export const TotalURL = `${host}breeds/total`

export function GetAPIdogInfoEndpoint(dogId) {
    return `${host}breeds/${dogId}`;
} 

export function GetAPIbreedsEndpoint(page=1, limit=16) {
    return `${host}breeds?page=${page}&limit=${limit}`;
}
