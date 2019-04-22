FROM node:11

ENV NODE_ENV=production

WORKDIR /app
COPY . /app

RUN yarn install --production

EXPOSE 3000
CMD yarn start
