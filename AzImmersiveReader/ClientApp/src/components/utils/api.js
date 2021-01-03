import axios from 'axios';

export function getTokenandSubdomainAsync() {
    return axios.get("/api/ImmersiveReader")
    .then(function (response) {
        console.log(response.data)
    })
    .catch(function(error) {
        console.log(error)
    })
}

export function handleLaunchImmersiveReader() {
    let token = ''
    let subdomain = ''
    return axios.get("/api/ImmersiveReader")
    .then(function (response) {
        token = response.data.token;
        subdomain = response.data.subdomain;
    })
    .catch(function(error) {
        console.log(error)
    })
    .then(()=>{
        console.log('Token: ' + token);
        console.log('Subdomain: ' + subdomain);
    })
}