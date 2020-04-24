# build stage
FROM node:alpine as build-stage

ARG vue_build_mode

WORKDIR /app

COPY package*.json ./

RUN npm install

#RUN npm rebuild node-sass

COPY . .

RUN npm run build.${vue_build_mode}

# production stage
FROM nginx:latest as production-stage

COPY --from=build-stage /app/dist /usr/share/nginx/html

COPY --from=build-stage /app/nginx.conf /etc/nginx/nginx.conf
