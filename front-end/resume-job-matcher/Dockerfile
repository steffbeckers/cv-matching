# build stage
FROM node:lts as build-stage

WORKDIR /app

COPY package*.json ./

RUN npm install

RUN npm rebuild node-sass

COPY . .

RUN npm run build.prod

# production stage
FROM nginx as production-stage

COPY --from=build-stage /app/dist/resume-job-matcher /usr/share/nginx/html

COPY --from=build-stage /app/nginx.conf /etc/nginx/nginx.conf
