FROM node:10 AS build

# copy
WORKDIR /app
COPY . .
RUN npm i && \
  npm run build

FROM node:10 AS runtime
WORKDIR /app
RUN npm i -g serve
COPY --from=build /app/build build
EXPOSE 3000
ENTRYPOINT ["serve", "-s", "build", "-l" , "3000"] 